using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WLED
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceListViewPage : ContentPage
    {
        [XmlElement("Devices")]
        private ObservableCollection<WLEDDevice> _Items;

        public ObservableCollection<WLEDDevice> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
                DeviceListView.ItemsSource = _Items;
                UpdateLabelVisibility();
            }
        }

    public DeviceListViewPage()
        {
            InitializeComponent();

            _Items = new ObservableCollection<WLEDDevice>
            {
                /*new WLEDDevice("10.10.1.11","Table", 1812052),
                new WLEDDevice("10.10.1.12","Clock", 0),
                new WLEDDevice("google.com","Sample Device", 0)*/
            };
			
			DeviceListView.ItemsSource = _Items;

            UpdateLabelVisibility();

            topMenuBar.SetButtonIcon(ButtonLocation.Right, "icon_add.png");
            topMenuBar.RightButtonTapped += On_AddButton_Tapped;
        }

        async void On_AddButton_Tapped(object sender, EventArgs e)
        {
            var page = new DeviceAddPage(this);
            page.DeviceCreated += On_Device_Created;
            await Navigation.PushModalAsync(page, false);
        }

        void On_Device_Created(object sender, DeviceCreatedEventArgs e)
        {
            if (e.CreatedDevice != null)
            {
                int index = 0;
                while (index < _Items.Count && e.CreatedDevice.IsGreaterThan(_Items.ElementAt(index))) index++;
                _Items.Insert(index, e.CreatedDevice);

                UpdateLabelVisibility();
            }
        }

        async void On_PowerButton_Tapped(object sender, ItemTappedEventArgs e)
        {
            Button s = sender as Button;
            WLEDDevice wd = s.Parent.BindingContext as WLEDDevice;
            await DisplayAlert("Item Tapped", wd.Name, "OK");
        }

        async void On_Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            WLEDDevice targetDevice = e.Item as WLEDDevice;

            string url = targetDevice.NetworkAddress;
            if (url == null) return;

            if (!url.StartsWith("http://")) url = "http://" +url;

            if (url.Length > 7)
            {
                var page = new DeviceControlPage(url);
                await Navigation.PushModalAsync(page, false);
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        void UpdateLabelVisibility()
        {
            bool listIsEmpty = (_Items.Count == 0);

            welcomeLabel.IsVisible = listIsEmpty;
            instructionLabel.IsVisible = listIsEmpty;

            if (listIsEmpty)
            {
                topMenuBar.SetButtonIcon(ButtonLocation.Left, null);
            } else
            {
                topMenuBar.SetButtonIcon(ButtonLocation.Left, "icon_modify.png");
            }
        }
    }
}
