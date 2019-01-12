using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WLED
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceModificationListViewPage : ContentPage
	{
        private ObservableCollection<WLEDDevice> DeviceList;
        public DeviceModificationListViewPage (ObservableCollection<WLEDDevice> items)
		{
			InitializeComponent ();

            DeviceList = items;
            DeviceModificationListView.ItemsSource = DeviceList;
        }

        private void OnDeleteButtonTapped(object sender, ItemTappedEventArgs e)
        {
            Button s = sender as Button;
            WLEDDevice targetDevice = s.Parent.BindingContext as WLEDDevice;
            if (targetDevice == null) return;

            DeviceList.Remove(targetDevice);
            if (DeviceList.Count == 0) Navigation.PopModalAsync(false);
        }

        private async void OnDeviceTapped(object sender, ItemTappedEventArgs e)
        {
            //Deselect Item immediately
            ((ListView)sender).SelectedItem = null;

            if (e.Item == null) return;
            WLEDDevice targetDevice = e.Item as WLEDDevice;
            if (targetDevice == null) return;

            string url = "http://" + targetDevice.NetworkAddress;

            var page = new DeviceControlPage(url, targetDevice);
            await Navigation.PushModalAsync(page, false);
        }
    }
}