using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace HTTPServer
{
    internal class RequestHandler
    {
        private readonly Thread thread;
        private ConcurrentQueue<Socket> socketQueue;
        public bool IsRunning { get; private set; } = false;

        public RequestHandler(ConcurrentQueue<Socket> socketQueue)
        {
            this.socketQueue = socketQueue;
            this.thread = new Thread(this.Run);
        }

        public void Start()
        {
            this.IsRunning = true;
            this.thread.Start();
        }

        public void Stop()
        {
            this.IsRunning = false;
        }

        private void Run()
        {
            while(this.IsRunning)
            {
                Socket socket;
                if(socketQueue.TryDequeue(out socket))
                {
                    this.ProcessSocket(socket);
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
        }

        private string GetRequest(Socket socket)
        {
            StringBuilder requestBuilder = new StringBuilder();
            byte[] inputData = new byte[256];
            int bytesRead;
            do
            {
                bytesRead = socket.Receive(inputData);
                string requestPart = Encoding.UTF8.GetString(inputData, 0, bytesRead);
                requestBuilder.Append(requestPart);
            }
            while (bytesRead > 0);

            string request = requestBuilder.ToString();
            return request;
        }

        public void ProcessSocket(Socket socket)
        {
            string request = GetRequest(socket);
            Console.WriteLine(request);
        }
    }
}
