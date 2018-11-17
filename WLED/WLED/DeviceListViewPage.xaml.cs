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
        public ObservableCollection<string> Items { get; set; }

        public DeviceListViewPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
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
            string tpdTest = e.Item as string;

            await DisplayAlert("Item Tapped", tpdTest + " was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
