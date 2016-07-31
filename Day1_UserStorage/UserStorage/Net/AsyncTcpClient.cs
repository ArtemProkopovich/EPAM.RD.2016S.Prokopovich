using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Net
{
    public class AsyncTcpClient
    {
        private readonly IPAddress address;
        private readonly int port;
        public AsyncTcpClient(IPAddress address, int port)
        {
            this.address = address;
            this.port = port;
        }

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
