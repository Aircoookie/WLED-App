using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WLED
{
    public partial class App : Application
    {

        private DeviceListViewPage main;

        public App()
        {
            InitializeComponent();

            main = new DeviceListViewPage();
            MainPage = main;
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
                    if (fromPreferences != null) main.Items = fromPreferences;
                }
            }
           
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps, save device list to Preferences
            string devices = Serialization.SerializeObject(main.Items);
            Preferences.Set("wleddevices", devices);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
