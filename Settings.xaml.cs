using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace UDP_TCP_S_R
{
    /// <summary>
    /// Interakční logika pro Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        SettingsC settingsC = new SettingsC();
        public static Settings st;

        public Settings()
        {
            InitializeComponent();
            st = this;
            InitializePlaceholder();
            settingsC.InitializeSettings();
        }

        Placeholder localUDP;
        Placeholder remoteUDP;
        void InitializePlaceholder()
        {
            localUDP = new Placeholder(st.DefaultLocalLabelUDP, st.DefaultLocalUDP);
            remoteUDP = new Placeholder(st.DefaultRemoteLabelUDP, st.DefaultRemoteUDP);
        }

        private void DefaultCheckboxUDP_Checked(object sender, RoutedEventArgs e)
        {
            settingsC.SaveSettingsUDP();
            DefaultLocalUDP.IsEnabled = true;
            DefaultRemoteUDP.IsEnabled = true;
        }

        private void DefaultCheckboxUDP_Unchecked(object sender, RoutedEventArgs e)
        {
            settingsC.SaveSettingsUDP();
            DefaultLocalUDP.IsEnabled = false;
            DefaultRemoteUDP.IsEnabled = false;
        }

        private void DefaultLocalUDP_LostFocus(object sender, RoutedEventArgs e)
        {
            Validation(DefaultLocalUDP);
            settingsC.SaveSettingsUDP();
        }

        void Validation(TextBox textBox)
        {
            if (int.TryParse(textBox.Text, out int val))
            {
                if (val > 63535)
                {
                    textBox.Text = 63535.ToString();
                }
                else if (val < 0)
                {
                    textBox.Text = 0.ToString();
                }
            }
            else
            {
                textBox.Text = 0.ToString();
            }
        }

        private void DefaultRemoteUDP_LostFocus(object sender, RoutedEventArgs e)
        {
            Validation(DefaultRemoteUDP);
            settingsC.SaveSettingsUDP();
        }
    }
}
