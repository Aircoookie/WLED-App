using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace WLED
{
    enum DeviceStatus { Connecting, Connected, Unreachable, Error };

    [XmlType("dev")]
    public class WLEDDevice : INotifyPropertyChanged
    {
        [XmlElement("url")]
        public string NetworkAddress { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        int Version { get; }

        [XmlElement("en")]
        public bool IsEnabled { get; set; } = true;

        [XmlIgnore]
        public bool IsShown { get; set; } = true;

        public WLEDDevice() { }

        public WLEDDevice(string nA, string name, int v)
        {
            NetworkAddress = nA;
            Name = name;
            Version = v;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsGreaterThan(WLEDDevice comp)
        {
            if (comp == null) return false;
            return (Name.CompareTo(comp.Name) > 0);
        }
    }
}
