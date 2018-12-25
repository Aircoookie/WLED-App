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
        private bool deletionMode = false;

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
                UpdateElementsVisibility();
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

            UpdateElementsVisibility();

            topMenuBar.SetButtonIcon(ButtonLocation.Right, ButtonIcon.Add);
            topMenuBar.LeftButtonTapped += On_DeleteButton_Tapped;
            topMenuBar.RightButtonTapped += On_AddButton_Tapped;
        }

        async void On_AddButton_Tapped(object sender, EventArgs e)
        {
            if (deletionMode) return;
            var page = new DeviceAddPage(this);
            page.DeviceCreated += On_Device_Created;
            await Navigation.PushModalAsync(page, false);
        }

        void On_DeleteButton_Tapped(object sender, EventArgs e)
        {
            deletionMode = !deletionMode;
            UpdateElementsVisibility();
        }

        void On_Device_Created(object sender, DeviceCreatedEventArgs e)
        {
            if (e.CreatedDevice != null)
            {
                int index = 0;
                while (index < _Items.Count && e.CreatedDevice.IsGreaterThan(_Items.ElementAt(index))) index++;
                _Items.Insert(index, e.CreatedDevice);

                UpdateElementsVisibility();
            }
        }

        async void On_PowerButton_Tapped(object sender, ItemTappedEventArgs e)
        {
            Button s = sender as Button;
            WLEDDevice targetDevice = s.Parent.BindingContext as WLEDDevice;
            if (targetDevice == null) return;

            string url = NetUtility.PrependHTTPToURL(targetDevice.NetworkAddress);
            if (url == null) return;

            await DeviceHTTPConnection.Send_WLED_API_Call(url, "T=2");
        }

        async void On_Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            WLEDDevice targetDevice = e.Item as WLEDDevice;
            if (targetDevice == null) return;

            if (deletionMode)
            {
                _Items.Remove(targetDevice);
                UpdateElementsVisibility();
                return;
            }

            string url = NetUtility.PrependHTTPToURL(targetDevice.NetworkAddress);
            if (url == null) return;

            var page = new DeviceControlPage(url);
            await Navigation.PushModalAsync(page, false);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        void UpdateElementsVisibility()
        {
            bool listIsEmpty = (_Items.Count == 0);

            welcomeLabel.IsVisible = listIsEmpty;
            instructionLabel.IsVisible = listIsEmpty;

            ButtonIcon toSet;

            if (listIsEmpty)
            {
                deletionMode = false;
                toSet = ButtonIcon.None;
            } else
            {
                toSet = deletionMode ? ButtonIcon.Back : ButtonIcon.Delete;
            }
            topMenuBar.SetButtonIcon(ButtonLocation.Left, toSet);
            topMenuBar.SetButtonIcon(ButtonLocation.Right, deletionMode ? ButtonIcon.None : ButtonIcon.Add);
        }
    }
}
