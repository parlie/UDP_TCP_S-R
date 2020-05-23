using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace UDP_TCP_S_R
{
    class UDPSender
    {
        private static Random random = new Random();
        private static UdpClient UDPServer; //Create UDPServer for whole class

        public static void UDPSend(IPAddress ip, int remot, string data) //Function for sending data
            {
                while (true)
                {
                    int sendingPort = random.Next(0, 63535);
                    if (!UDPReceiver.portValues.Contains(sendingPort))
                    {
                        UDPServer = new UdpClient(sendingPort);
                        byte[] DataToSend = new byte[4]; //Variable for storing data to send
                        var RemoteEndPoint = new IPEndPoint(ip, remot); //Remote IPEndPoint - IP address of receiver + port
                        DataToSend = Encoding.ASCII.GetBytes(data); //converts string of data to bytes
                        UDPServer.Send(DataToSend, DataToSend.Length, RemoteEndPoint); //Sends data to specific EndPoint
                        if (MainWindow.DataVar != null)
                        {
                            Log.WriteInfo(MainWindow.DataVar);
                        }
                        UDPServer.Close();
                        break;
                    }
                }
            }
    }
}
