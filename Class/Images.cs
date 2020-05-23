using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UDP_TCP_S_R
{
    public class Images
    {
        public static byte[] ImageToByte(Image image)
        {
            ImageConverter imageConverter = new ImageConverter();
            return (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
        }

        public static Image StringToImage(string strincc)
        {
            ImageConverter imageConverter = new ImageConverter();
            return (Image)imageConverter.ConvertFromInvariantString(strincc);
        }

        public static Image BytesToImage(byte[] byt)
        {
            ImageConverter imageConverter = new ImageConverter();
            return (Image)imageConverter.ConvertFrom(byt);
        }

        public static void ImageSend(byte[] image)
        {
           // TCPClient.Connect(server,image);
        }

        public static void ImageReceive()
        {

        }
    }
}
