using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace WLED
{
    enum DeviceStatus { Connecting, Connected, Unreachable, Error };

    [XmlType("dev")]
    public class WLEDDevice
    {
        [XmlElement("url")]
        public string NetworkAddress { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        int Version { get; }

        public WLEDDevice() { }

        public WLEDDevice(string nA, string name, int v)
        {
            NetworkAddress = nA;
            Name = name;
            Version = v;
        }

        public bool IsGreaterThan(WLEDDevice comp)
        {
            if (comp == null) return false;
            return (Name.CompareTo(comp.Name) > 0);
        }
    }
}
