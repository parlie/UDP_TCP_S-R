using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace UDP_TCP_S_R
{
    public class TCPClient
    {
        public static void Connect(string server, string message)
        {
            int port = 13000;
            //create client
            TcpClient client = new TcpClient(server,port);

            //translate message
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            //client stream for reading and writing
            NetworkStream stream = client.GetStream();

            //send message to connected server
            stream.Write(data, 0, data.Length);
            Log.WriteInfo("Sent: " + message);

            data = new byte[256];
            string responseData = String.Empty;
            int bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Log.WriteResponse("Received: " + responseData);

            stream.Close();
            client.Close();

        }

        public static void Connect(string server, byte[] data)
        {

        }

    }
}
