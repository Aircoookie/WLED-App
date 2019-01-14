using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WLED
{
    //Viewmodel: Page for adding new lights
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceAddPage : ContentPage
	{
        public event EventHandler<DeviceCreatedEventArgs> DeviceCreated;
        private bool discoveryMode = false;
        private int devicesFoundCount = 0;

		public DeviceAddPage(DeviceListViewPage list)
		{
			InitializeComponent ();

            topMenuBar.SetButtonIcon(ButtonLocation.Right, ButtonIcon.Done);
            topMenuBar.RightButtonTapped += OnEntryCompleted;

            networkAddressEntry.Focus();
        }

        //If done, create device and close page
        private async void OnEntryCompleted(object sender, EventArgs e)
        {
            if (sender is Entry currentEntry) currentEntry.Unfocus();

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

            //Add device, but not if the user clicked checkmark after doing auto-discovery only
            if (devicesFoundCount == 0 || !address.Equals("192.168.4.1")) OnDeviceCreated(new DeviceCreatedEventArgs(device));
        }

        private void OnDiscoveryButtonClicked(object sender, EventArgs e)
        {
            discoveryMode = !discoveryMode;
            Button b = sender as Button;
            if (b == null) return;
            var discovery = DeviceDiscovery.GetInstance();
            if (discoveryMode)
            {
                //Start mDNS discovery
                b.Text = "Stop discovery";
                devicesFoundCount = 0;
                discovery.ValidDeviceFound += OnDeviceCreated;
                discoveryResultLabel.IsVisible = true;
                discoveryResultLabel.Text = "Found no lights yet...";
                discovery.StartDiscovery();
            } else
            {
                //Stop mDNS discovery
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

            //DeviceDiscovery already made an API request to confirm that the new device is a WLED light,
            //so a refresh is only required for manually added devices
            RefreshRequired = refresh;
        }
    }
}