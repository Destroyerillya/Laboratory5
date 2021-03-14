using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpSample
{
    class Chat
    {
        private static int localPort = 15000;
        private static string nickName;
        private static string msg;
        private static string datasend;
        private static string fullmsg;
        private static UdpClient udpClient = new UdpClient();

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                udpClient.ExclusiveAddressUse = false;
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, localPort));
                Console.WriteLine("Enter your Nickname:");
                nickName = Console.ReadLine();
                Thread tRec = new Thread(new ThreadStart(Receiver));
                tRec.Start();
                while (true)
                {
                    msg = Console.ReadLine();
                    switch (msg)
                    {
                        case "exit":
                            Environment.Exit(0);
                            break;
                        default:
                            datasend = DateTime.Now.ToString();
                            fullmsg = datasend + ":" + nickName + "-" + msg;
                            Send(fullmsg);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        private static void Send(string datagram)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(datagram);
                udpClient.Send(data, data.Length,IPAddress.Broadcast.ToString() , localPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        public static void Receiver()
        {
            try
            {
                var from = new IPEndPoint(0, 0);
                Console.WriteLine("\n-----------*******Chat*******-----------");
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    Console.WriteLine(Encoding.UTF8.GetString(recvBuffer));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString() + "\n  " + ex.Message);
            }
        }
    }
}