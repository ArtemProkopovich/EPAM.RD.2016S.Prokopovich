using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Net
{
    /// <summary>
    /// Simple async tcp client
    /// </summary>
    public class AsyncTcpClient
    {
        private readonly IPAddress address;
        private readonly int port;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">Address for connection</param>
        /// <param name="port">Port for connection</param>
        public AsyncTcpClient(IPAddress address, int port)
        {
            this.address = address;
            this.port = port;
        }

        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(byte[] message)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(address, port);
            using (var networkStream = client.GetStream())
            {
                await networkStream.WriteAsync(message, 0, message.Length);
            }
            client.Close();
        }
    }
}
