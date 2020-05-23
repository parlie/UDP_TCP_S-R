using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Documents;

namespace UDP_TCP_S_R
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        SettingsC settingsC = new SettingsC();

        public MainWindow()
        {
            InitializeComponent();
            mw = this; //Refernce to access UI 
            InitializePlaceholder();
            settingsC.Initialize();
            MyIpAddress.Content = "Local IP: " + Network.GetMyIp();
            UpdateListbox();
        }

        private static void UpdateListbox()
        {
            mw.saveListBox.Items.Clear();
            foreach (var v in XML.XMLRead())
            {
               mw.saveListBox.Items.Add(v + Environment.NewLine);
            }
        }

        void InitializePlaceholder()
        {
            placeholderIP = new Placeholder(mw.IPAddressLabel, mw.IPInput);
            placeholderLocalPort = new Placeholder(mw.LocalPortLabel, mw.LocalPortInput);
            placeholderRemotePort = new Placeholder(mw.RemotePortLabel, mw.RemotePortInput);
            placeholderData = new Placeholder(mw.DataLabel, mw.DataInput);

            placeholderIPTCP = new Placeholder(mw.IPAdressLabelTcp, mw.tcpIpAdress);
            placeholderLocalPortTcp = new Placeholder(mw.LocalPortLabelTcp, mw.tcpLocalPort);
            placeholderRemotePortTcp = new Placeholder(mw.RemotePortLabelTcp, mw.tcpRemotePort);
            placeholderDataTcp = new Placeholder(mw.DataLabelTcp, mw.tcpData);

        }

        #region VariableDefinitions

        Placeholder placeholderIP;
        Placeholder placeholderLocalPort;
        Placeholder placeholderRemotePort;
        Placeholder placeholderData;

        Placeholder placeholderIPTCP;
        Placeholder placeholderLocalPortTcp;
        Placeholder placeholderRemotePortTcp;
        Placeholder placeholderDataTcp;
        //TODO: Settings - Port validation

        //User data input
        private static IPAddress IPAdressVar; //Stores IP provided by user
        public static int RemotePortVar; //Stores remote port provided by user
        public static int LocalPortVar; //Stores local port provided by user
        public static string DataVar; //stores data to send provided by user

        static IPAddress ipAdressVarTcp;
        static int remotePortVarTcp;
        static int localPortVarTcp;
        static string dataVarTcp;

        //UDP
        UDPReceiver server;

        //TCP
        TCPServer tcpServer;

        //Other data
        public static Tuple<AppTheme, Accent> appStyle;
        public enum theme { night, day };

        public static MainWindow mw; //Variable for accesing UI

        private static bool stop = false;

        #endregion

        #region DataImputToVar

        //UDP

        private void LocalPortInput_LostFocus(object sender, RoutedEventArgs e) //Adds data from input field to variable - Local Port
        {
            int localPortLenght = LocalPortInput.Text.Length;
            if (localPortLenght != 0)
            {
                if (int.TryParse(LocalPortInput.Text, out int localPortInput))
                {
                    if(localPortInput >= 0 && localPortInput <= 63535)
                    {
                        LocalPortVar = localPortInput;
                    }
                    else
                    {
                        Log.WriteError("This value is not in range!");
                        if(localPortInput > 63535)
                        {
                            LocalPortInput.Text = 63535.ToString();
                            LocalPortVar = 63535;
                        }
                        else if(localPortInput < 0)
                        {
                            LocalPortInput.Text = 0.ToString();
                            LocalPortVar = 0;
                        }
                    }
                }
                else
                {
                    Log.WriteError("Invalid port value!");
                    LocalPortInput.Text = "";
                }
            }
        }

        private void DataInput_LostFocus(object sender, RoutedEventArgs e) //Adds data from input field to variabel - Data
        {
                if (DataInput.Text.Length > 0) //Prevents error to show up on empty field
                {
                    DataVar = DataInput.Text;
                }
        }

        private void IPInput_LostFocus(object sender, RoutedEventArgs e) //Adds data from input field to variable - IP address
        {
            int localIPLenght = IPInput.Text.Length;
            if (localIPLenght != 0)
            {
                if (IPAddress.TryParse(IPInput.Text, out IPAddress localIP))
                {
                    IPAdressVar = localIP;
                }
                else
                {
                    Log.WriteError( "Invalid IP adress!");
                    IPInput.Text = "";
                }
            }
        }

        private void RemotePortInput_LostFocus(object sender, RoutedEventArgs e) //Adds data from input field to variable - Remote Port
        {
            int remotePortLenght = RemotePortInput.Text.Length;
            if (remotePortLenght != 0)
            {
                if (int.TryParse(RemotePortInput.Text, out int remotePortInput))
                {
                    if (remotePortInput >= 0 && remotePortInput <= 63535)
                    {
                        RemotePortVar = remotePortInput;
                    }
                    else
                    {
                        Log.WriteError("This value is not in range!");
                        if(remotePortInput > 63535)
                        {
                            RemotePortInput.Text = 63535.ToString();
                            RemotePortVar = 63535;
                        }
                        else if(remotePortInput < 0)
                        {
                            RemotePortInput.Text = 0.ToString();
                            RemotePortVar = 0;
                        }
                    }
                }
                else
                {
                    Log.WriteError("Invalid port value!");
                    RemotePortInput.Text = "";
                }
            }
        }

        //TCP

        private void tcpLocalPort_LostFocus(object sender, RoutedEventArgs e)
        {
            int localPortLenght = tcpLocalPort.Text.Length;
            if(localPortLenght != 0)
            {
                if(int.TryParse(tcpLocalPort.Text,out int localPortInput))
                {
                    if(localPortInput >= 0 && localPortInput <= 63535)
                    {
                        localPortVarTcp = localPortInput;
                    }
                    else
                    {
                        Log.WriteError("This value is not in range!");
                        if(localPortInput > 63535)
                        {
                            tcpLocalPort.Text = 63535.ToString();
                            localPortVarTcp = 63535;
                        }
                        else if(localPortInput < 0)
                        {
                            tcpLocalPort.Text = 0.ToString();
                            localPortVarTcp = 0;
                        }
                    }
                }
                else
                {
                    Log.WriteError("Invalid port value!");
                    tcpLocalPort.Text = 0.ToString();
                    localPortVarTcp = 0;
                }
            }
        }

        private void tcpData_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tcpData.Text.Length > 0)
            {
                dataVarTcp = tcpData.Text;
            }
        }

        private void tcpIpAdress_LostFocus(object sender, RoutedEventArgs e)
        {
            int localIPLenght = tcpIpAdress.Text.Length;
            if(localIPLenght != 0)
            {
                if(IPAddress.TryParse(tcpIpAdress.Text,out IPAddress localIP))
                {
                    ipAdressVarTcp = localIP;
                }
                else
                {
                    Log.WriteError("Invalid IP adress!");
                    tcpIpAdress.Text = "";
                }
            }
        }

        private void tcpRemotePort_LostFocus(object sender, RoutedEventArgs e)
        {
            int remotePortLenght = tcpRemotePort.Text.Length;
            if(remotePortLenght != 0)
            {
                if(int.TryParse(tcpRemotePort.Text, out int remotePortInput))
                {
                    if(remotePortInput >= 0 && remotePortInput <= 63535)
                    {
                        remotePortVarTcp = remotePortInput;
                    }
                    else
                    {
                        Log.WriteError("This value is not in range!");
                        if(remotePortInput > 63535)
                        {
                            tcpRemotePort.Text = 63535.ToString();
                            remotePortVarTcp = 63535;
                        }
                        else if (remotePortInput < 0)
                        {
                            tcpRemotePort.Text = 0.ToString();
                            remotePortVarTcp = 0;
                        }
                    }
                }
                else
                {
                    Log.WriteError("Invalid port value!");
                    tcpRemotePort.Text = "";
                }
            }
        }


        #endregion

        //UDP Control

        #region ButtonClicksUDP

        private void Button_Click(object sender, RoutedEventArgs e) //Send
        {
            if (DataVar != null && RemotePortVar >= 0 && RemotePortVar <= 63535 && IPAdressVar != null)
            {
                UDPSender.UDPSend(IPAdressVar, RemotePortVar, DataVar);
            }
            else if(DataVar == null)
            {
                Log.WriteError("No data to send.");
            }
            else if(RemotePortVar < 0 || RemotePortVar > 63535)
            {
                Log.WriteError("Invalid port.");
            }
            else if(IPAdressVar == null)
            {
                Log.WriteError("Invalid IP adress");
            }
            else
            {
                Log.WriteError( "There was an error while sending data!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //Listen
        {
                server = new UDPReceiver(LocalPortVar);
                server.InitializeUDPServer();
                stop = true;
        }
        
        private void Button_Click_2(object sender, RoutedEventArgs e) //Save
        {
            if (RemotePortVar >= 0 && LocalPortVar >= 0 && IPAdressVar != null)
            {
                XML.XMLCreate(IPAdressVar, RemotePortVar, LocalPortVar);
                UpdateListbox();
            }
            else
            {
                Log.WriteError("No data to store.");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) //Load
        {
            if (saveListBox.SelectedIndex >= 0)
            {
                int listID = saveListBox.SelectedIndex;
                IPAddress ipvar = IPAddress.Parse(XML.GiveIP(listID).ToString());
                int remotevar = XML.GiveRemotePort(listID);
                int localvar = XML.GiveLocalPort(listID);
                IPAdressVar = ipvar;
                IPInput.Text = ipvar.ToString();

                RemotePortVar = remotevar;
                RemotePortInput.Text = remotevar.ToString();

                LocalPortVar = localvar;
                LocalPortInput.Text = localvar.ToString();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e) //Stop
        {
            if (stop)
            {
                server.Stop();
                Log.WriteSucces("UDP server has been stoped.");
                stop = false;
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e) //Delete
        {
                File.Delete(XML.full);
                Log.WriteSucces("File has been deleted.");
                XML.Val();
                UpdateListbox();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e) //Log terminal
        {
            LogRichBox.SelectAll();
            LogRichBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, FontSize = 24);
            FileStream fs = new FileStream("C:\\Users\\Public\\Documents\\UDPData\\terminal.rtf", FileMode.Create, FileAccess.Write);
            LogRichBox.Selection.Save(fs, DataFormats.Rtf);
            LogRichBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, FontSize = 12);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e) //SendASCII
        {
            OpenFileDialog ofd = new OpenFileDialog();
            List<string> list = new List<string>();
            DataVar = null;
            DataInput.Text = null; ;
            if (IPAdressVar != null)
            {
                if (ofd.ShowDialog() == true)
                {
                      Log.WriteInfo( "File: " + ofd.FileName + " has been sent to: " + IPAdressVar + ":" + RemotePortVar);
                      foreach (var v in File.ReadLines(ofd.FileName))
                      {
                          UDPSender.UDPSend(IPAdressVar, RemotePortVar, v);
                          Thread.Sleep(1000);
                      }
                }
            }
            else
            {
                Log.WriteError( "Missing IP adress.");
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e) //ClearLog
        {
            LogRichBox.SelectAll();
            LogRichBox.Selection.Text = "";
        }

        #endregion

        //TCP Control

        #region ButtonClickTCP

        private void Button_Click_9(object sender, RoutedEventArgs e) //Listen
        {
            tcpServer = new TCPServer(localPortVarTcp);
            tcpServer.StartServer();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e) //Send
        {

        }

        private void Button_Click_11(object sender, RoutedEventArgs e) //Stop
        {
            if (tcpServer != null)
            {
                if (tcpServer.IsTCPServerRunning)
                {
                    tcpServer.StopServer();
                }
            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e) //SendASCIIImage
        { 
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Image files|*.jpg;*.png;*.bmp";
            ofd.InitialDirectory = "c:\\";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;
            ofd.Title = "Select image to send";
            ofd.Multiselect = false;

            Nullable<bool> ofdResult = ofd.ShowDialog();

            if(ofdResult == true)
            {
                byte[] image = Images.ImageToByte(Image.FromFile(ofd.FileName));
                Images.ImageSend(image);
            }
        }

        private void Button_Click_13(object sender, RoutedEventArgs e) //Load
        {

        }

        private void Button_Click_14(object sender, RoutedEventArgs e) //Save
        {

        }

        private void Button_Click_15(object sender, RoutedEventArgs e) //Delete
        {

        }

        #endregion

        //Settings

        #region SettingsClicks


        #endregion

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ChangeTheme("BaseDark", Application.Current);
            settingsC.SaveAccentAndTheme();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ChangeTheme("BaseLight", Application.Current);
            settingsC.SaveAccentAndTheme();
        }

        public static void ChangeTheme(string theme, Application app)
        {
            appStyle = ThemeManager.DetectAppStyle(app);
            switch (theme)
            {
                case "BaseLight":
                    ThemeManager.ChangeAppStyle(app, appStyle.Item2, ThemeManager.GetAppTheme(theme));
                    mw.moon.Kind = PackIconFontAwesomeKind.MoonRegular;
                    break;
                case "BaseDark":
                    ThemeManager.ChangeAppStyle(app, appStyle.Item2, ThemeManager.GetAppTheme(theme));
                    mw.moon.Kind = PackIconFontAwesomeKind.MoonSolid;
                    break;
                default:
                    ThemeManager.ChangeAppStyle(app, appStyle.Item2, ThemeManager.GetAppTheme(theme));
                    mw.moon.Kind = PackIconFontAwesomeKind.MoonRegular;
                    break;
            }
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            Settings s = new Settings();
            s.Show();
        }

        private void sbuton_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ChangeAccent(sbuton.SelectedItem.ToString(), Application.Current);
            settingsC.SaveAccentAndTheme();
        }

        public static void ChangeAccent(string accent, Application app)
        {
            appStyle = ThemeManager.DetectAppStyle(app);
            CurrentItem(accent);
            ThemeManager.ChangeAppStyle(app, ThemeManager.GetAccent(accent), appStyle.Item1);
        }

        private static void CurrentItem(string value)
        {
            MainWindow.mw.sbuton.SelectedItem = value;
            MainWindow.mw.sbuton.SelectedValue = value;
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\Users\\Public\\Documents\\UDPData\\terminal.rtf" );
        }

        private void LocalPortInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }

    }
}
