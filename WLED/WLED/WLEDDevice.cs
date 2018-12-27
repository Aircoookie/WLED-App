using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WLED
{
    enum DeviceStatus { Default, Unreachable, Error };

    [XmlType("dev")]
    public class WLEDDevice : INotifyPropertyChanged, IComparable
    {
        private string networkAddress;
        private string name = "";
        private DeviceStatus status = DeviceStatus.Default;
       
        [XmlElement("url")]
        public string NetworkAddress
        {
            set
            {
                networkAddress = value;
            }
            get
            {
                return networkAddress;
            }
        }

        [XmlElement("name")]
        public string Name
        { 
            set
            {
                if (value == null) return; //make sure name is not set to null
                name = value;
                OnPropertyChanged("Name");
            }
            get
            {
                return name;
            }
        }

        internal DeviceStatus CurrentStatus
        {
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
            get
            {
                return status;
            }
        }

        [XmlElement("namecustom")]
        public bool NameIsCustom { get; set; } = true;

        [XmlElement("en")]
        public bool IsEnabled { get; set; } = true;

        [XmlIgnore]
        public bool IsShown { get; set; } = true;

        [XmlIgnore]
        public double brightnessCurrent { get; set; }

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

        public async void SendAPICall(string call)
        {
            System.Diagnostics.Debug.WriteLine("Call");
            string url = "http://" + networkAddress;

            string response = await DeviceHttpConnection.GetInstance().Send_WLED_API_Call(url, call);
            if (response == null)
            {
                CurrentStatus = DeviceStatus.Unreachable;
            }
            else if (response.Equals("err"))
            {
                CurrentStatus = DeviceStatus.Error;
            }
            else
            {
                CurrentStatus = DeviceStatus.Default;
                if (!NameIsCustom) Name = XmlApiResponseParser.GetNameFromResponse(response);
            }
            System.Diagnostics.Debug.WriteLine("Call Done");
        }

        public void Refresh()
        {
            //fetches updated values from WLED device
            SendAPICall("");
            System.Diagnostics.Debug.WriteLine("Refresh Done");
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
