using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Drawing;

namespace UDP_TCP_S_R
{
    class TCPServer
    {
        //Variables
        Socket listener;
        List<Socket> clients = new List<Socket>();
        byte[] buffer;
        int localPort;
        static List<int> portValues = new List<int>();
        public bool IsTCPServerRunning = false;

        public TCPServer(int _localPort)
        {
            localPort = _localPort;
        }

        public void StartServer()
        {
            if (!portValues.Contains(localPort))
            {
                IsTCPServerRunning = true;
                portValues.Add(localPort);
                Log.WriteSucces($"TCP server is now listenig on port {localPort}");
                listener = new Socket(AddressFamily.Unspecified, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(new IPEndPoint(IPAddress.Any, localPort));
                listener.Listen(5);
                Log.WriteInfo("Awaiting clients...");
                if (IsTCPServerRunning)
                {
                    listener.BeginAccept(AcceptCallback, null);
                }
                else
                {
                    StopServer();
                }
            }
            else
            {
                Log.WriteError($"TCP server is already running on port {localPort}, please use diferent port.");
            }
        }

        public void StopServer()
        {
            listener.Dispose();
            for (int i = 0; i < clients.Count; i++)
            {
                clients[0].Disconnect(true);
            }
            portValues.Remove(localPort);
            IsTCPServerRunning = false;
            Log.WriteSucces("Server has been succesfuly closed and all clients disconnected.");
        }

        void AcceptCallback(IAsyncResult result)
        {
            try
            {
                Socket connected = listener.EndAccept(result);
                clients.Add(connected);
                MainWindow.mw.Dispatcher.Invoke(() =>
                {
                    Log.WriteInfo($"Client connected from {connected.RemoteEndPoint}");
                });
                buffer = new byte[connected.ReceiveBufferSize];
                if (IsTCPServerRunning)
                {
                    connected.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, connected);
                    listener.BeginAccept(AcceptCallback, null);
                }
                else
                {
                    StopServer();
                }
            }
            catch (ObjectDisposedException e)
            {
                MainWindow.mw.Dispatcher.Invoke(() =>
                {
                    Log.WriteError("Current socket was disposed, voiding accept callback.");
                });
            }
        }

        void ReceiveCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            int bytes = socket.EndReceive(result);
            if (bytes != 0)
            {
                if(buffer.First() != 2)
                {
                    MainWindow.mw.Dispatcher.Invoke(() =>
                    {
                      //  Log.WriteInfo($"Received {bytes} bytes from {socket.RemoteEndPoint}");
                        Log.WriteResponse(Encoding.UTF8.GetString(buffer, 0, bytes));
                    });
                    if (IsTCPServerRunning)
                    {
                        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, socket);
                    }
                    else
                    {
                        StopServer();
                    }
                }
                else
                {
                    MainWindow.mw.Dispatcher.Invoke(() =>
                    {
                        Log.WriteInfo("Receiving image...");
                    });
                    byte[] temp;
                    string ext = null;
                    temp = new byte[buffer.Length - 1];
                    Array.Copy(buffer, 1,temp, 0, buffer.Length - 1);
                    if(temp.First() == 0)
                    {
                        ext = "jpg";
                    }
                    else if(temp.First() == 1)
                    {
                        ext = "png";
                    }
                    else if(temp.First() == 2)
                    {
                        ext = "bmp";
                    }
                    buffer = new byte[temp.Length - 1];
                    Array.Copy(temp, 1, buffer, 0, temp.Length - 1);
                    Image image = Images.BytesToImage(buffer);
                    while (true)
                    {
                        if (Directory.Exists(@"C:\Users\Public\Pictures\Received Images\"))
                        {
                            if (ext == "jpg")
                            {
                                image.Save(@"C:\Users\Public\Pictures\Received Images\" + DateTime.Now.ToString("MM-dd-yyyy hh-mm") + "." + ext, System.Drawing.Imaging.ImageFormat.Jpeg);
                                MainWindow.mw.Dispatcher.Invoke(() =>
                                {
                                    Log.WriteSucces("Image succesfully received!");
                                });
                            }
                            else if (ext == "png")
                            {
                                image.Save(@"C:\Users\Public\Pictures\Received Images\" + DateTime.Now.ToString("MM-dd-yyyy hh-mm") + "." + ext, System.Drawing.Imaging.ImageFormat.Png);
                                MainWindow.mw.Dispatcher.Invoke(() =>
                                {
                                    Log.WriteSucces("Image succesfully received!");
                                });
                            }
                            else if (ext == "bmp")
                            {
                                image.Save(@"C:\Users\Public\Pictures\Received Images\" + DateTime.Now.ToString("MM-dd-yyyy hh-mm") + "." + ext, System.Drawing.Imaging.ImageFormat.Bmp);
                                MainWindow.mw.Dispatcher.Invoke(() =>
                                {
                                    Log.WriteSucces("Image succesfully received!");
                                });
                            }
                            else
                            {
                                MainWindow.mw.Dispatcher.Invoke(() =>
                                {
                                    Log.WriteError("There was an error while saving data.");
                                });
                            }
                            break;
                        }
                        else
                        {
                            Directory.CreateDirectory(@"C:\Users\Public\Pictures\Received Images\");
                        }
                    }
                }
            }
            else
            {
                clients.Remove(socket);
                MainWindow.mw.Dispatcher.Invoke(() =>
                {
                    Log.WriteInfo($"Client {socket.RemoteEndPoint} disconnected.");
                });
            }
        }

    }
    
}
