using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WLED
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceControlPage : ContentPage
	{
        private WLEDDevice currentDevice;

		public DeviceControlPage (string pageURL, WLEDDevice device)
		{
			InitializeComponent ();
            currentDevice = device;
            UIBrowser.Source = pageURL;
            UIBrowser.Navigated += On_NavigationCompleted;
        }

        private void On_NavigationCompleted(object sender, WebNavigatedEventArgs e)
        {
            if (e.Result == WebNavigationResult.Success)
            {
                loadingLabel.IsVisible = false;
                if (currentDevice != null) currentDevice.CurrentStatus = DeviceStatus.Default;
            } else
            {
                if (currentDevice != null) currentDevice.CurrentStatus = DeviceStatus.Unreachable;
                loadingLabel.IsVisible = true;
                loadingLabel.Text = "Device Unreachable";
            }
        }
    }
}