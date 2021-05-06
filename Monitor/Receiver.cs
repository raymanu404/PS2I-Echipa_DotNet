using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Comm
{
    public class Receiver
    {
        private TcpListener tcpListener;
        public EventHandler DataReceived;
        public Receiver(String address,int port)
        {
            tcpListener = new TcpListener(IPAddress.Parse(address), port);
        }
        public void StartListener()
        {
           
            try
            {
                tcpListener.Start();
                Byte[] bytes = new Byte[256];
                while (true)
                {
                    Console.WriteLine("I'm waiting for a new connection... ");
                    TcpClient client = tcpListener.AcceptTcpClient();

                    Console.WriteLine("You are connected...");

                    NetworkStream stream = client.GetStream();
                    int i = 0;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var values = bytes[0];
                        if(DataReceived != null)
                        {
                            DataReceived(values, null);
                        }
                    }

                    stream.Close();
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
