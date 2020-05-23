using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UDP_TCP_S_R
{

    [Serializable]
    public class SettingsC
    {
        public string Accent { get; set; }
        public string Theme { get; set; }
        public bool UseDefaultPortUDP { get; set; }
        public bool UseDefaultPortTCP { get; set; }
        public int DefaultLocalPortUDP { get; set; }
        public int DefaultRemotePortUDP { get; set; }
        public int DefaultLocalPortTCP { get; set; }
        public int DefaultRemotePortTCP { get; set; }
        private static string path = @"C:\\Users\\Public\\Documents\\UDPData\\settings.json";
        List<string> settings = new List<string>();
        private string[] acc = { "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" };
        private string[] thm = { "BaseLight", "BaseDark" };

        public void Initialize()
        {
            MainWindow.mw.sbuton.ItemsSource = acc;


            if (!File.Exists(path))
            {
                MainWindow.appStyle = ThemeManager.DetectAppStyle(Application.Current);
                SettingsC settings = new SettingsC()
                {
                    Accent = MainWindow.appStyle.Item2.Name,
                    Theme = MainWindow.appStyle.Item1.Name,
                };

                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            else
            {
                SettingsC settings = JsonConvert.DeserializeObject<SettingsC>(File.ReadAllText(path));
                MainWindow.ChangeAccent(settings.Accent,Application.Current);
                MainWindow.ChangeTheme(settings.Theme, Application.Current);
                if(settings.UseDefaultPortUDP)
                {
                    MainWindow.RemotePortVar = settings.DefaultRemotePortUDP;
                    MainWindow.mw.RemotePortInput.Text = settings.DefaultRemotePortUDP.ToString();
                    MainWindow.LocalPortVar = settings.DefaultLocalPortUDP;
                    MainWindow.mw.LocalPortInput.Text = settings.DefaultLocalPortUDP.ToString();
                }
            }
        }

        public void InitializeSettings()
        {
            SettingsC settingsC = JsonConvert.DeserializeObject<SettingsC>(File.ReadAllText(path));
            Settings.st.DefaultLocalUDP.Text = settingsC.DefaultLocalPortUDP.ToString();
            Settings.st.DefaultRemoteUDP.Text = settingsC.DefaultRemotePortUDP.ToString();
            if (settingsC.UseDefaultPortUDP)
            {
                Settings.st.DefaultCheckboxUDP.IsChecked = settingsC.UseDefaultPortUDP;
            }
            else
            {
                Settings.st.DefaultLocalUDP.IsEnabled = false;
                Settings.st.DefaultRemoteUDP.IsEnabled = false;
            }
        }

        public void SaveAccentAndTheme()
        {
            MainWindow.appStyle = ThemeManager.DetectAppStyle(Application.Current);
            SettingsC setting = JsonConvert.DeserializeObject<SettingsC>(File.ReadAllText(path));
            
            if (!setting.Accent.Contains(MainWindow.appStyle.Item2.Name))
            {
                setting.Accent = MainWindow.appStyle.Item2.Name;
            }
            if (!setting.Theme.Contains(MainWindow.appStyle.Item1.Name))
            {
                setting.Theme = MainWindow.appStyle.Item1.Name;
            }
            
            string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public void SaveSettingsUDP()
        {
            MainWindow.appStyle = ThemeManager.DetectAppStyle(Application.Current);
            SettingsC setting = JsonConvert.DeserializeObject<SettingsC>(File.ReadAllText(path));

            setting.UseDefaultPortUDP = Settings.st.DefaultCheckboxUDP.IsChecked.Value;

            if (Settings.st.DefaultLocalUDP.Text.Length >= 1 && setting.DefaultLocalPortUDP != int.Parse(Settings.st.DefaultLocalUDP.Text))
            {
                setting.DefaultLocalPortUDP = int.Parse(Settings.st.DefaultLocalUDP.Text);
            }
            if(Settings.st.DefaultRemoteUDP.Text.Length >= 1 && setting.DefaultRemotePortUDP != int.Parse(Settings.st.DefaultRemoteUDP.Text))
            {
               setting.DefaultRemotePortUDP = int.Parse(Settings.st.DefaultRemoteUDP.Text);
            }
            setting.Accent = MainWindow.appStyle.Item2.Name;
            setting.Theme = MainWindow.appStyle.Item1.Name;

            string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public void SaveSettingsTCP()
        {

        }
    }
}
