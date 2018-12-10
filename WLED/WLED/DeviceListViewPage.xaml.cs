using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WLED
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceListViewPage : ContentPage
    {
        public ObservableCollection<WLEDDevice> Items { get; set; }

        public DeviceListViewPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<WLEDDevice>
            {
                new WLEDDevice("10.10.1.11","Table", 1812052),
                new WLEDDevice("10.10.1.12","Clock", 0)
            };
			
			DeviceListView.ItemsSource = Items;
        }

        async void Handle_MenuItem_Activated(object sender, ItemTappedEventArgs e)
        {
            await DisplayAlert("Item Tapped", "Menu was tapped.", "OK");
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            WLEDDevice tpdTest = e.Item as WLEDDevice;

            string url = "http://";
            url += tpdTest.NetworkAddress;

            //Device.OpenUri(new Uri(url));
            //await DisplayAlert("Item Tapped", tpdTest.Name + " was tapped.", "OK");
            var page = new DeviceControlPage(url);
            await Navigation.PushModalAsync(page);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
