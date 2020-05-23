using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_TCP_S_R
{
    class UDPReceiver
    {
        public UdpClient UDPServer;
        public bool stop = false;
        public int localPort;
        public static List<int> portValues = new List<int>();
        private static Random random = new Random();


        public UDPReceiver(int _localPort)
        {
            localPort = _localPort;
        }

        void CallbackReceive(IAsyncResult result)
        {
            if (!stop)
            {
                var remoteEndPoint = new IPEndPoint(IPAddress.Any, localPort);
                byte[] receivedData = UDPServer.EndReceive(result, ref remoteEndPoint);
                MainWindow.mw.Dispatcher.Invoke(() =>
                {
                    Log.WriteResponse(Encoding.ASCII.GetString(receivedData) + " - on port: " + localPort);

                });
                if (!stop)
                {
                    UDPServer.BeginReceive(new AsyncCallback(CallbackReceive), null);
                }
                else
                {
                    UDPServer.Close();
                }
            }
        }

        public void InitializeUDPServer()
        {
            if (!portValues.Contains(localPort))
            {
                stop = false;
                UDPServer = new UdpClient(localPort);
                Log.WriteSucces("UDP server is now running on port: " + localPort);
                portValues.Add(localPort);
                UDPServer.BeginReceive(new AsyncCallback(CallbackReceive), null);
            }
            else
            {
                Log.WriteError("UDP server is already running on this port!");
            }
        }

        public void Stop()
        {
            if (portValues.Contains(localPort))
            {
                stop = true;
                UDPServer.Close();
                portValues.Remove(localPort);
            }
        }

    }
}
