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
        bool listIsEmpty;

        public DeviceListViewPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<WLEDDevice>
            {
                /*new WLEDDevice("10.10.1.11","Table", 1812052),
                new WLEDDevice("10.10.1.12","Clock", 0),
                new WLEDDevice("google.com","Sample Device", 0),
                new WLEDDevice("google.com","Sample Device", 0),
                new WLEDDevice("google.com","Sample Device", 0),
                new WLEDDevice("google.com","Sample Device", 0),
                new WLEDDevice("google.com","Sample Device", 0),
                new WLEDDevice("google.com","Sample Device", 0),
                new WLEDDevice("google.com","Sample Device", 0),
                new WLEDDevice("google.com","Sample Device", 0),
                new WLEDDevice("google.com","Sample Device", 0),
                new WLEDDevice("google.com","Sample Device", 0)*/
            };
			
			DeviceListView.ItemsSource = Items;

            listIsEmpty = (Items.Count == 0);

            topMenuBar.SetButtonIcon(ButtonLocation.Left, "");
            topMenuBar.SetButtonIcon(ButtonLocation.Right, "icon_add.png");
            topMenuBar.RightButtonTapped += On_AddButton_Tapped;
        }

        async void On_AddButton_Tapped(object sender, EventArgs e)
        {
            var page = new DeviceAddPage(this);
            await Navigation.PushModalAsync(page, false);
        }

        async void On_PowerButton_Tapped(object sender, ItemTappedEventArgs e)
        {
            Button s = sender as Button;
            WLEDDevice wd = s.Parent.BindingContext as WLEDDevice;
            await DisplayAlert("Item Tapped", wd.Name, "OK");
        }

        async void On_Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            WLEDDevice targetDevice = e.Item as WLEDDevice;

            string url = "http://";
            url += targetDevice.NetworkAddress;

            var page = new DeviceControlPage(url);
            await Navigation.PushModalAsync(page, false);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
