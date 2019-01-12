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
        public bool IsEnabled { get; set; } = true;

        [XmlIgnore]
        public double BrightnessCurrent { get; set; }

        [XmlIgnore]
        public bool StateCurrent
        {
            get { return stateCurrent; }
            set { OnPropertyChanged("StateColor"); stateCurrent = value; }
        }

        [XmlIgnore]
        public Color StateColor { get { return StateCurrent ? Color.FromHex("#666") : Color.FromHex("#222"); } }

        public string Status
        {
            get
            {
                string statusText = "";
                switch (status)
                {
                    case DeviceStatus.Default: statusText = ""; break;
                    case DeviceStatus.Unreachable: statusText = " (Offline)"; break;
                    case DeviceStatus.Error: statusText = " (Error)"; break;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> SendAPICall(string call)
        {
            System.Diagnostics.Debug.WriteLine("Call");
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
                BrightnessCurrent = deviceResponse.Brightness;
                StateCurrent = deviceResponse.State;
                return true;
            }
        }

        public async Task<bool> Refresh()
        {
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
