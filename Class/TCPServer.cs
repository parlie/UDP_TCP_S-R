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
    class TCPServer
    {
        int localPort;
        static List<int> portValues = new List<int>();

        public bool IsTCPServerRunning = false;
        Socket listener = new Socket(AddressFamily.Unspecified, SocketType.Stream, ProtocolType.Tcp);
        byte[] buffer;

        public TCPServer(int _localPort)
        {
            localPort = _localPort;
        }

        void Begin()
        {
            listener.Bind(new IPEndPoint(IPAddress.Any, localPort));
            listener.Listen(1);
            MainWindow.mw.Dispatcher.Invoke(() =>
            {
                Log.WriteInfo("Awaiting client connection.");
            });
            if (IsTCPServerRunning)
            {
                listener.BeginAccept(AcceptCallback, null);
            }
        }

        void AcceptCallback(IAsyncResult result)
        {
            if (IsTCPServerRunning)
            {
                Socket socket = listener.EndAccept(result);
                buffer = new byte[256];
                MainWindow.mw.Dispatcher.Invoke(() =>
                {
                    Log.WriteInfo("Client succesfully connected.");
                });
                // endPoint.Create(socket.RemoteEndPoint.Serialize());
                if (IsTCPServerRunning)
                {
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, socket);
                }
            }
            else
            {
                listener.Close();
            }
        }

        void ReceiveCallback(IAsyncResult result)
        {
            Console.WriteLine(result.IsCompleted);
            Socket socket = (Socket)result.AsyncState;
            int size = socket.EndReceive(result);
            if (size == 0)
            {
                StopServer(result);
            }
            else
            {
                string data = (string)Encoding.ASCII.GetString(buffer);
                MainWindow.mw.Dispatcher.Invoke(() =>
                {
                    Log.WriteResponse(data);
                    Log.WriteInfo($"Recieved {size} bytes.");
                });
                buffer = new byte[256];
                if (socket.Connected)
                {
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, socket);
                }
            }
        }

        public void StartServer()
        {
            if (!portValues.Contains(localPort))
            {
                // listener.ReceiveTimeout = 10000;
                IsTCPServerRunning = true;
                Log.WriteSucces("TCP server is now running on port: " + localPort);
                portValues.Add(localPort);
                Begin();
            }
            else
            {
                Log.WriteError("TCP server is already running on this port!");
            }
        }

        public void StopServer(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            socket.Close();
            if (portValues.Contains(localPort))
            {
                try
                {
                    int read = 0;
                    while ((read = listener.Receive(buffer)) > 0)
                    { }
                }
                catch
                {
                    //ignore
                }
                listener.Close();
            }
            MainWindow.mw.Dispatcher.Invoke(() =>
            {
                Log.WriteError("Remote client has disconnected.");
            });
            IsTCPServerRunning = false;
            portValues.Remove(localPort);
        } 

        public void StopServer()
        {
            listener.Close();
            IsTCPServerRunning = false;
        }
    }
    
}
