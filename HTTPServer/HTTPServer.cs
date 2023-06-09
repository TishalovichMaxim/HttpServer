using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Http
{
    public class HttpServer
    {
        public string Host
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }

        public HttpServer(string host, int port) 
        {
            this.Host = host;
            this.Port = port;
        }

        public void Run()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(this.Host), this.Port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(10);
            Console.WriteLine("Waiting for connections...");

            while (true)
            {
                Socket clientSocket = socket.Accept();
                
                string message = "Hello, world";
                byte[] data = Encoding.UTF8.GetBytes(message);
                clientSocket.Send(data);

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
    }
}
