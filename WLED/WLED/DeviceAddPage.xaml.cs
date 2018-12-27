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
	public partial class DeviceAddPage : ContentPage
	{
        public event EventHandler<DeviceCreatedEventArgs> DeviceCreated;

		public DeviceAddPage (DeviceListViewPage list)
		{
			InitializeComponent ();

            topMenuBar.SetButtonIcon(ButtonLocation.Right, ButtonIcon.Done);
            topMenuBar.RightButtonTapped += Entry_Completed;

            networkAddressEntry.Focus();
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            var currentEntry = sender as Entry;
            if (currentEntry != null) currentEntry.Unfocus();

            var device = new WLEDDevice();

            string address = networkAddressEntry.Text;
            string name = nameEntry.Text;

            if (address == null || address.Length == 0) address = "192.168.4.1";
            if (address.StartsWith("http://")) address = address.Substring(7);
            if (address.EndsWith("/")) address = address.Substring(0, address.Length -1);
            if (name == null || name.Length == 0)
            {
                name = "(New Light)";
                device.NameIsCustom = false;
            }

            device.Name = name;
            device.NetworkAddress = address;

            await Navigation.PopModalAsync(false);
            OnDeviceCreated(new DeviceCreatedEventArgs(device));
        }

        protected virtual void OnDeviceCreated(DeviceCreatedEventArgs e)
        {
            EventHandler<DeviceCreatedEventArgs> handler = DeviceCreated;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class DeviceCreatedEventArgs
    {
        public WLEDDevice CreatedDevice { get; }

        public DeviceCreatedEventArgs(WLEDDevice created)
        {
            CreatedDevice = created;
        }
    }
}