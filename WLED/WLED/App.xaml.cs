using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WLED
{
    public partial class App : Application
    {

        private DeviceListViewPage main;

        private bool connectedToLocalLast = false;

        public App()
        {
            InitializeComponent();

            main = new DeviceListViewPage();
            MainPage = main;
            Connectivity.ConnectivityChanged += On_ConnectivityChanged;
        }

        protected override void OnStart()
        {
            // Load device list from Preferences
            if (Preferences.ContainsKey("wleddevices"))
            {
                string devices = Preferences.Get("wleddevices", "");
                if (!devices.Equals(""))
                {
                    ObservableCollection<WLEDDevice> fromPreferences = Serialization.Deserialize(devices);
                    if (fromPreferences != null) main.DeviceList = fromPreferences;
                }
            }
           
        }

        protected override void OnSleep()
        {
            // Handle when app sleeps, save device list to Preferences
            string devices = Serialization.SerializeObject(main.DeviceList);
            Preferences.Set("wleddevices", devices);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            main.RefreshAll();
        }

        private void On_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var profiles = Connectivity.ConnectionProfiles;
            bool connectedToLocal = (profiles.Contains(ConnectionProfile.WiFi) || profiles.Contains(ConnectionProfile.Ethernet));
            if (connectedToLocal && !connectedToLocalLast) main.RefreshAll();
            connectedToLocalLast = connectedToLocal;
        }
    }
}
