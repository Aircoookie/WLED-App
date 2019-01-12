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
        private bool discoveryMode = false;
        private int devicesFoundCount = 0;

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

            //add device, but not if the user clicked checkmark after doing auto-discovery only
            if (devicesFoundCount == 0 || !address.Equals("192.168.4.1")) OnDeviceCreated(new DeviceCreatedEventArgs(device));
        }

        private void On_DiscoveryButtonClicked(object sender, EventArgs e)
        {
            discoveryMode = !discoveryMode;
            Button b = sender as Button;
            if (b == null) return;
            var discovery = DeviceDiscovery.GetInstance();
            if (discoveryMode)
            {
                b.Text = "Stop discovery";
                devicesFoundCount = 0;
                discovery.ValidDeviceFound += OnDeviceCreated;
                discoveryResultLabel.IsVisible = true;
                discoveryResultLabel.Text = "Found no lights yet...";
                discovery.StartDiscovery();
            } else
            {
                discovery.StopDiscovery();
                discovery.ValidDeviceFound -= OnDeviceCreated;
                b.Text = "Discover lights...";
            }      
        }

        protected virtual void OnDeviceCreated(DeviceCreatedEventArgs e)
        {
            DeviceCreated?.Invoke(this, e);
        }

        private void OnDeviceCreated(object sender, DeviceCreatedEventArgs e)
        {
            //this method only gets called by mDNS search, display found devices
            devicesFoundCount++;
            if (devicesFoundCount == 1)
            {
                discoveryResultLabel.Text = "Found " + e.CreatedDevice.Name + "!";
            } else
            {
                discoveryResultLabel.Text = "Found " + e.CreatedDevice.Name + " and " + (devicesFoundCount - 1) + " other lights!";
            }

            OnDeviceCreated(e);
        }

        protected override void OnDisappearing()
        {
            //stop discovery if running
            if (discoveryMode)
            {
                var discovery = DeviceDiscovery.GetInstance();
                discovery.StopDiscovery();
                discovery.ValidDeviceFound -= OnDeviceCreated;
            }
        }
    }

    public class DeviceCreatedEventArgs
    {
        public WLEDDevice CreatedDevice { get; }
        public bool RefreshRequired { get; } = true;

        public DeviceCreatedEventArgs(WLEDDevice created, bool refresh = true)
        {
            CreatedDevice = created;
            RefreshRequired = refresh;
        }
    }
}