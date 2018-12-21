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

        private void NetworkAddressEntry_Completed(object sender, EventArgs e)
        {
            nameEntry.Focus();
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            string address = networkAddressEntry.Text;
            string name = nameEntry.Text;

            if (address == null || address.Length == 0) address = "192.168.4.1";
            if (name == null || name.Length == 0) name = "New Device";

            var device = new WLEDDevice(address, name, 0);

            OnDeviceCreated(new DeviceCreatedEventArgs(device));
            await Navigation.PopModalAsync(false);
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