using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace WLED
{
    enum DeviceStatus { Default, Unreachable, Error };

    [XmlType("dev")]
    public class WLEDDevice : INotifyPropertyChanged, IComparable
    {
        private string networkAddress;
        private string name = "";
        private DeviceStatus status = DeviceStatus.Default;
        private bool stateCurrent = false;
        private bool isEnabled = true;

        [XmlElement("url")]
        public string NetworkAddress
        {
            set { networkAddress = value; }
            get { return networkAddress; }
        }

        [XmlElement("name")]
        public string Name
        { 
            set
            {
                if (value == null || name.Equals(value)) return; //make sure name is not set to null
                name = value;
                OnPropertyChanged("Name");
            }
            get { return name; }
        }

        internal DeviceStatus CurrentStatus
        {
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
            get { return status; }
        }

        [XmlElement("ncustom")]
        public bool NameIsCustom { get; set; } = true;

        [XmlElement("en")]
        public bool IsEnabled
        {
            set
            {
                isEnabled = value;
                OnPropertyChanged("Status");
                OnPropertyChanged("ListHeight");
                OnPropertyChanged("TextColor");
                OnPropertyChanged("IsEnabled");
            }
            get { return isEnabled; }
        }

        [XmlIgnore]
        public double BrightnessCurrent { get; set; } = 0.9;

        [XmlIgnore]
        public bool StateCurrent
        {
            get { return stateCurrent; }
            set { OnPropertyChanged("StateColor"); stateCurrent = value; }
        }

        [XmlIgnore]
        public Color StateColor { get { return StateCurrent ? Color.FromHex("#666") : Color.FromHex("#222"); } }

        [XmlIgnore]
        public string ListHeight { get { return isEnabled ? "-1" : "0"; } }

        [XmlIgnore]
        public string TextColor { get { return isEnabled ? "#FFF" : "#999"; } }

        public string Status
        {
            get
            {
                string statusText = "";
                if (IsEnabled)
                {
                    switch (status)
                    {
                        case DeviceStatus.Default: statusText = ""; break;
                        case DeviceStatus.Unreachable: statusText = " (Offline)"; break;
                        case DeviceStatus.Error: statusText = " (Error)"; break;
                    }
                } else
                {
                    statusText = " (Hidden)";
                }
                return string.Format("{0}{1}", networkAddress, statusText);
            }
        }

        public WLEDDevice() { }

        public WLEDDevice(string nA, string name)
        {
            NetworkAddress = nA;
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            System.Diagnostics.Debug.Write("pC");
            System.Diagnostics.Debug.WriteLine(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> SendAPICall(string call)
        {
            string url = "http://" + networkAddress;

            string response = await DeviceHttpConnection.GetInstance().Send_WLED_API_Call(url, call);
            if (response == null)
            {
                CurrentStatus = DeviceStatus.Unreachable;
                return false;
            }
            else if (response.Equals("err"))
            {
                CurrentStatus = DeviceStatus.Error;
                return false;
            }
            else
            {
                CurrentStatus = DeviceStatus.Default;
                XmlApiResponse deviceResponse = XmlApiResponseParser.ParseApiResponse(response);
                if (deviceResponse == null) return false;
                if (!NameIsCustom) Name = deviceResponse.Name;
                if (deviceResponse.Brightness > 0)
                {
                    BrightnessCurrent = deviceResponse.Brightness; //only account for brightness if light is on
                    OnPropertyChanged("BrightnessCurrent");
                }
                StateCurrent = deviceResponse.State;
                return true;
            }
        }

        public async Task<bool> Refresh()
        {
            if (!IsEnabled) return false;
            //fetches updated values from WLED device
            return await SendAPICall("");
        }

        public int CompareTo(object comp)
        {
            WLEDDevice c = comp as WLEDDevice;
            if (c == null || c.Name == null) return 1;
            int result = (name.CompareTo(c.name));
            if (result != 0) return result;
            return (networkAddress.CompareTo(c.networkAddress));
        }
    }
}
