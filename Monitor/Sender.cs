using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Comm
{
    public class Sender
    {
        private TcpClient tcpClient;
       
        public Sender(string address,int port)
        {
            tcpClient = new TcpClient(address,port);
        }
        public void Send(byte value)
        {
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                byte[] data = new byte[4];
                data[0] = value;
                stream.Write(data, 0, data.Length);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
