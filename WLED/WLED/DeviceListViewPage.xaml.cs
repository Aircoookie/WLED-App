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
        private ObservableCollection<WLEDDevice> _DeviceList;
        private bool deletionMode = false;

        public ObservableCollection<WLEDDevice> DeviceList
        {
            get
            {
                return _DeviceList;
            }
            set
            {
                _DeviceList = value;
                DeviceListView.ItemsSource = _DeviceList;
                RefreshAll();
                UpdateElementsVisibility();
            }
        }

        public DeviceListViewPage()
        {
            InitializeComponent();

            DeviceList = new ObservableCollection<WLEDDevice>();

            Resources["PowerButtonImageStyle"] = Resources["PowerButtonImageStylePower"];
            topMenuBar.SetButtonIcon(ButtonLocation.Right, ButtonIcon.Add);
            topMenuBar.LeftButtonTapped += On_DeletionModeButton_Tapped;
            topMenuBar.RightButtonTapped += On_AddButton_Tapped;
        }

        private void On_Refresh(object sender, EventArgs e)
        {
            RefreshAll();
            DeviceListView.EndRefresh();
        }

        private async void On_AddButton_Tapped(object sender, EventArgs e)
        {
            if (deletionMode) return;
            var page = new DeviceAddPage(this);
            page.DeviceCreated += On_Device_Created;
            await Navigation.PushModalAsync(page, false);
        }

        private void On_DeletionModeButton_Tapped(object sender, EventArgs e)
        {
            deletionMode = !deletionMode;
            Resources["PowerButtonImageStyle"] = Resources[deletionMode? "PowerButtonImageStyleDelete" : "PowerButtonImageStylePower"];
            UpdateElementsVisibility();
        }

        private void Insert_Device_Sorted(WLEDDevice d)
        {
            int index = 0;
            while (index < _DeviceList.Count && d.CompareTo(_DeviceList.ElementAt(index)) > 0) index++;
            _DeviceList.Insert(index, d);
        }

        private void On_Device_Created(object sender, DeviceCreatedEventArgs e)
        {
            WLEDDevice toAdd = e.CreatedDevice;

            if (toAdd != null)
            {
                foreach (WLEDDevice d in _DeviceList)
                {
                    //ensure there is only one device entry per IP
                    if (toAdd.NetworkAddress.Equals(d.NetworkAddress))
                    {
                        if (toAdd.NameIsCustom)
                        {
                            d.Name = toAdd.Name;
                            d.NameIsCustom = true;
                        }
                        return;
                    }
                }
                Insert_Device_Sorted(toAdd);

                toAdd.PropertyChanged += Device_PropertyChanged;
                if (e.RefreshRequired) toAdd.Refresh();

                UpdateElementsVisibility();
            }
        }

        private void Device_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (((PropertyChangedEventArgs)e).PropertyName.Equals("Name"))
            {
                Reinsert_Device_Sorted(sender as WLEDDevice);
            }
        }

        private void Reinsert_Device_Sorted(WLEDDevice d)
        {
            if (d == null) return;
            if (_DeviceList.Remove(d)) Insert_Device_Sorted(d);
        }

        private void On_PowerButton_Tapped(object sender, ItemTappedEventArgs e)
        {
            Button s = sender as Button;
            WLEDDevice targetDevice = s.Parent.BindingContext as WLEDDevice;
            if (targetDevice == null) return;

            if (deletionMode) //power button functions as delete button
            {
                _DeviceList.Remove(targetDevice);
                UpdateElementsVisibility();
                return;
            }

            targetDevice.SendAPICall("T=2");
        }

        private async void On_Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            //Deselect Item immediately
            ((ListView)sender).SelectedItem = null;

            if (e.Item == null) return;
            WLEDDevice targetDevice = e.Item as WLEDDevice;
            if (targetDevice == null) return;

            if (deletionMode) //toggle device visibility in list
            {
                //not implemented
                return;
            }

            string url = "http://" + targetDevice.NetworkAddress;

            var page = new DeviceControlPage(url, targetDevice);
            await Navigation.PushModalAsync(page, false);
        }

        internal async void OpenAPDeviceControlPage()
        {
            var page = new DeviceControlPage("http://192.168.4.1", null);
            await Navigation.PushModalAsync(page, false);
        }

        private void UpdateElementsVisibility()
        {
            bool listIsEmpty = (_DeviceList.Count == 0);

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

        internal void RefreshAll()
        {
            foreach (WLEDDevice d in _DeviceList)
            {
                d.Refresh();
            }
        }
    }
}
