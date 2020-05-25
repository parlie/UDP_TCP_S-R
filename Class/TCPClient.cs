using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace UDP_TCP_S_R
{
    public class TCPClient
    {
        static Random random = new Random();
        Socket sender;
        int remotePort;
        IPAddress iPAddress;

        public TCPClient(int _RemotePort, IPAddress _iPAddress)
        {
            remotePort = _RemotePort;
            iPAddress = _iPAddress;
        }

        public void SendData(string data)
        {

        }

        public void SendImage()
        {

        }
    }
}
