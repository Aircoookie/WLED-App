using System;
using System.Collections.Generic;
using System.Text;

namespace WLED
{
    enum DeviceStatus { Connecting, Connected, Unreachable, Error };

    public class WLEDDevice
    {
        public string NetworkAddress { get; set; }
        public string Name { get; set; }
        int Version { get; }

        public WLEDDevice(string nA, string name, int v)
        {
            NetworkAddress = nA;
            Name = name;
            Version = v;
        }
    }
}
