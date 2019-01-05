using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WLED
{
    class NetUtility
    {
        //we just assume we are connected to embedded AP if:
        //1. the IP is in 192.168.4.0/24 subnet
        //2. the device IP is between 2 and 5 (ESP8266 DHCP range) 
        public static bool IsConnectedToWledAP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    string ips = ip.ToString();
                    
                    if (ips.StartsWith("192.168.4."))
                    {
                        int dev = 0;
                        if (!Int32.TryParse(ips.Substring(10), out dev)) { dev = 0; }
                        if (dev > 1 && dev < 6) return true;
                    }
                }
            }
            return false;
        }
    }
}