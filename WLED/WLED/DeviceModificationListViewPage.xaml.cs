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
    //Viewmodel: Page for hiding and deleting existing device list entries
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

        private void OnDeleteButtonTapped(object sender, EventArgs e)
        {
            Button s = sender as Button;
            if (!(s.Parent.BindingContext is WLEDDevice targetDevice)) return;

            DeviceList.Remove(targetDevice);

            //Go back to main device list view if no devices in list
            if (DeviceList.Count == 0) Navigation.PopModalAsync(false);
        }

        private void OnDeviceTapped(object sender, ItemTappedEventArgs e)
        {
            //Deselect Item immediately
            ((ListView)sender).SelectedItem = null;

            if (e.Item is WLEDDevice targetDevice)
            {
                //Toggle Device enabled (disabled = hidden in list)
                targetDevice.IsEnabled = !targetDevice.IsEnabled;
            }
        }
    }
}