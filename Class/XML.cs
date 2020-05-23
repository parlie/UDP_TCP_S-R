using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;

namespace UDP_TCP_S_R
{
    public static class XML
    {
        public static string path = @"C:\\Users\\Public\\Documents\\UDPData";
        private static string fileName = "udpdata.xml";
        public static string full = path + "\\" + fileName;

        static string settingsPath = @"C:\\Users\\Public\\Documents\\UDPData\\settings.xml";

        private static XDocument fileXml;
        private static IPAddress trash;

        /// <summary>
        /// Saves data in XML file.
        /// </summary>
        /// <param name="ipAdress">IP adress</param>
        /// <param name="remotePort">Remote port</param>
        /// <param name="localPort">Local port</param>
        public static void XMLCreate(System.Net.IPAddress ipAdress, int remotePort, int localPort)
        {
            if (!File.Exists(full))
            {
                fileXml = new XDocument(new XDeclaration("1.0", Encoding.UTF8.HeaderName, ""), new XElement("List"));
            }
            else
            {
                fileXml = XDocument.Load(full);
            }
            fileXml.Element("List").Add(
            new XElement("Table",
            new XAttribute("ip", ipAdress), new XAttribute("remotePort", remotePort), new XAttribute("localPort", localPort))
            );
            if (!Directory.Exists(path))
            {
                 Directory.CreateDirectory(path);
            }
            Log.WriteSucces("Data saved succesfuly.");

            fileXml.Save(full);
        }

        private static XElement GetXML()
        {
            return XElement.Load(full);
        }

        public static List<string> XMLRead()
        {
            if(File.Exists(full))
            {
                List<string> tables = new List<string>();
                XElement values = GetXML();
                foreach(var v in values.Elements("Table"))
                {
                    tables.Add(v.Attribute("ip").Value + "," + v.Attribute("remotePort").Value + "," + v.Attribute("localPort").Value);
                }
                return tables;
            }
            else
            {
                Val();   
                return null;
               // Log.Write("No file to read.", Log.HighlightColor.Red);
            }

        }

        public static void Val()
        {
            fileXml = new XDocument(new XDeclaration("1.0", Encoding.UTF8.HeaderName, ""), new XElement("List"));
            fileXml.Save(full);
        }

        public static IPAddress GiveIP(int ID)
        {
                XElement values = GetXML();
                List<IPAddress> iplist = new List<IPAddress>();
                foreach (var v in values.Elements("Table"))
                {
                    if (IPAddress.TryParse(v.Attribute("ip").Value, out trash))
                    {
                        iplist.Add(IPAddress.Parse(v.Attribute("ip").Value));
                    }
                }
                return iplist[ID];
            
        }

        public static int GiveRemotePort(int ID)
        {
            XElement values = GetXML();
            List<int> remotePortList = new List<int>();
            foreach (var v in values.Elements("Table"))
            {
                int val;
                if(int.TryParse(v.Attribute("remotePort").Value, out val))
                {
                    if(val >= 0 && val <= 63535)
                    {
                        remotePortList.Add(val);
                    }
                    else if(val < 0)
                    {
                        Log.WriteError("There was an error while reading data.");
                        return 0;
                    }
                    else if(val > 63535)
                    {
                        Log.WriteError("There was an error while reading data.");
                        return 63535;
                    }
                }
                else
                {
                    Log.WriteError("There was an error while reading data.");
                    return 0;
                }
            }
            return remotePortList[ID];
        }

        public static int GiveLocalPort(int ID)
        {
            XElement values = GetXML();
            List<int> localPortList = new List<int>();
            foreach (var v in values.Elements("Table"))
            {
                int val;
                if (int.TryParse(v.Attribute("localPort").Value, out val))
                {
                    if (val >= 0 && val <= 63535)
                    {
                        localPortList.Add(val);
                    }
                    else if (val < 0)
                    {
                        Log.WriteError("There was an error while reading data.");
                        return 0;
                    }
                    else if (val > 63535)
                    {
                        Log.WriteError("There was an error while reading data.");
                        return 63535;
                    }
                }
                else
                {
                    Log.WriteError("There was an error while reading data.");
                    return 0;
                }
            }
            return localPortList[ID];
        }


        //Settings fragment

        public static void LoadSettings()
        {

        }

        public static void SaveSettings(string key, string value)
        {
            JObject jObject =
                 new JObject(
                     new JProperty("settings",
                     new JObject(
                     new JProperty("items",
                     new JArray(
                         new JObject(
                             new JProperty(key, value)
                             )
                     ))
                     )));
            Console.WriteLine(jObject);
            
        }


        public enum OfType {UDP,TCP};
        public static void SaveAdresses(OfType ofType)
        {
            switch (ofType)
            {
                case OfType.UDP:
                    foreach(var s in MainWindow.mw.saveListBox.Items)
                    {

                    }
                    break;
                case OfType.TCP:
                    foreach(var s in MainWindow.mw.saveListBoxTCP.Items)
                    {

                    }
                    break;
                default:

                    break;
            }
        }
    }
}
