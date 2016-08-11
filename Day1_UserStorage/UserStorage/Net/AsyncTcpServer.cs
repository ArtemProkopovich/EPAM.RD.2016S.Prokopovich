using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Net
{
    /// <summary>
    /// Simple async tcp server
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncTcpServer<T>
    {
        private readonly IPAddress address;
        private readonly int port;
        private readonly Func<Stream, Task<T>> processFunc;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">Address for connection</param>
        /// <param name="port">Port for connection</param
        /// <param name="processFunc">Func that process receive message</param>
        public AsyncTcpServer(IPAddress address, int port, Func<Stream,Task<T>> processFunc)
        {
            this.address = address;
            this.port = port;
            this.processFunc = processFunc;
        }

        /// <summary>
        /// Start listening on current address and port
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            TcpListener listener = new TcpListener(address, port);
            listener.Start();
            while (true)
            {
                TcpClient tcpClient = null;
                try
                {
                    tcpClient = await listener.AcceptTcpClientAsync();
                    await ProccessMessage(tcpClient);
                    tcpClient.Close();
                }
                finally
                {
                    tcpClient?.Close();
                }
            }
        }

        /// <summary>
        /// Process receive message
        /// </summary>
        /// <param name="client">Client that send message</param>
        /// <returns></returns>
        public async Task<T> ProccessMessage(TcpClient client)
        {
            using (var networkStream = client.GetStream())
            using (var ms = new MemoryStream())
            {
                int bytesRead;
                byte[] buffer = new byte[1024];
                do
                {
                    bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                    await ms.WriteAsync(buffer, 0, bytesRead);
                } while (bytesRead == buffer.Length);
                return await processFunc(ms);
            }
        }
    }
}
