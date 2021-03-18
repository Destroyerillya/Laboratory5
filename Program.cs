using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        private static bool running = true;

        static void Main(string[] args)
        {
            udpClient.ExclusiveAddressUse = false;
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, localPort));
            Console.WriteLine("Enter your Nickname:");
            nickName = Console.ReadLine();
            Task tRec = ReceiverTask();
            while (running)
            {
                msg = Console.ReadLine();
                switch (msg)
                {
                    case "exit":
                        running = false;
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

        private static void Send(string datagram)
        {
            var data = Encoding.UTF8.GetBytes(datagram);
            udpClient.Send(data, data.Length,IPAddress.Broadcast.ToString() , localPort);
        }
        
        static async Task ReceiverTask()
        {
            await Task.Run(Receiver);
        }

        public static async Task Receiver()
        {
            while(running)
            {
                var from = new IPEndPoint(0, 0);
                Console.WriteLine("\n-----------*******Chat*******-----------");
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    Console.WriteLine(Encoding.UTF8.GetString(recvBuffer));
                }
            }
        }
    }
}