using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace UDP_TCP_S_R
{
    class TCPClient
    {
        static Random random = new Random();
        public bool isClientConnected = false;
        int remotePort;
        IPAddress iPAddress;
        Socket client;
        byte[] buffer;

        public TCPClient(int _RemotePort, IPAddress _iPAddress)
        {
            remotePort = _RemotePort;
            iPAddress = _iPAddress;
        }

        public void ConnectToServer()
        {
            client = new Socket(AddressFamily.Unspecified, SocketType.Stream, ProtocolType.Tcp);
            client.BeginConnect(new IPEndPoint(iPAddress,remotePort), ConnectCallback, null);
        }

        void ConnectCallback(IAsyncResult result)
        {
            client.EndConnect(result);
            if(client.Connected)
            {
                isClientConnected = true;
                MainWindow.mw.Dispatcher.Invoke(() =>
                {
                    Log.WriteSucces($"Succesfully connected to {iPAddress}:{remotePort}");
                });
            }
        }

        public void SendData(string data)
        {
            buffer = new byte[data.Length];
            buffer = Encoding.UTF8.GetBytes(data);
            client.Send(buffer); 
            MainWindow.mw.Dispatcher.Invoke(() =>
            {
                Log.WriteSucces($"Succesfuly sent message '{data}' to {client.RemoteEndPoint}");
            });
        }

        public void CloseConnection()
        {
            EndPoint ep = client.RemoteEndPoint;
            client.Disconnect(true);
            client.Dispose();
            MainWindow.mw.Dispatcher.Invoke(() =>
            {
                Log.WriteInfo($"Client has disconnected from {ep}.");
            });
        }

        public void SendImage(byte[] image)
        {
            byte[] send = new byte[image.Length + 1];
            send[0] = 2;
            image.CopyTo(send, 1);
            client.Send(send);
            Log.WriteSucces($"Image has been succesfuly sent to {client.RemoteEndPoint}");
        }
    }
}
