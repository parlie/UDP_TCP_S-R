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
            listener.ReceiveTimeout = 10000;
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
                if (IsTCPServerRunning)
                {
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, socket);
                }
            }
            else
            {
                listener.Disconnect(true);
                listener.Close();
                listener.Dispose();
            }
        }

        void ReceiveCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            int size = socket.EndReceive(result);
            if (size == 0 || IsTCPServerRunning == false)
            {
                StopServer(result);
            }
            else
            {
                string data = (string)Encoding.ASCII.GetString(buffer);
                MainWindow.mw.Dispatcher.Invoke(() =>
                {
                    Log.WriteResponse(data);
                  //  Log.WriteInfo($"Recieved {size} bytes.");
                });
                buffer = new byte[256];
                if (socket.Connected)
                {
                    if (IsTCPServerRunning == true)
                    {
                        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, socket);
                    }
                    else
                    {
                        socket.Disconnect(true);
                    }
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
            socket.Disconnect(true);
            socket.Close();
            if (portValues.Contains(localPort))
            {
                //listener.Disconnect(true);
                listener.Close();
            }
            MainWindow.mw.Dispatcher.Invoke(() =>
            {
                Log.WriteError("Remote client has disconnected.");
            });
            portValues.Remove(localPort);
            IsTCPServerRunning = false;
        } 

        public void StopServer()
        {
            //listener.Disconnect(true);
            if(listener.Connected == true)
            {
                listener.Disconnect(true);
                listener.Close();
                listener.Dispose();
            }
            else
            {
                listener.Close();
                listener.Dispose();
                portValues.Remove(localPort);
                IsTCPServerRunning = false;
                Log.WriteSucces("TCP server has been succesfuly closed.");
            }
        }
    }
    
}
