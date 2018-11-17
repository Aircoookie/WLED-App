using System;
using System.Collections.Generic;
using System.Text;

namespace WLED
{
    public class Device
    {
        string NetworkAddress { get; set; }
        string Name { get; set; }
        int Version { get; }
    }
}
