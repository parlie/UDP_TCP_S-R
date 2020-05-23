using System;
using System.Net;
using System.Net.Sockets;

namespace UDP_TCP_S_R
{
    class Network
    {
        public static string GetMyIp()
        { 
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
