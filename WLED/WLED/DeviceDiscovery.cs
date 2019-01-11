using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tmds.MDns;

namespace WLED
{
    class DeviceDiscovery
    {
        private static DeviceDiscovery Instance;
        private ServiceBrowser serviceBrowser;
        private int devicesFound = 0;
        public event EventHandler<DeviceCreatedEventArgs> ValidDeviceFound;
        public event EventHandler<DiscoveryResultEventArgs> DiscoveryResult;

        private DeviceDiscovery()
        {
            try
            {
                serviceBrowser = new ServiceBrowser();
                serviceBrowser.ServiceAdded += OnServiceAdded;
            } catch
            {
                
            }
        }

        public async void StartDiscovery()
        {
            devicesFound = 0;
            try
            {
                serviceBrowser.StartBrowse("_http._tcp");
            }
            catch
            {
                OnDiscoveryEnd(new DiscoveryResultEventArgs(false, "The search could not be initialized"));
            }
        }

        public void StopDiscovery()
        {
            serviceBrowser?.StopBrowse();
            OnDiscoveryEnd(new DiscoveryResultEventArgs(true, "Dicovered a total of " + devicesFound + " WLED lights", devicesFound));
        }

        private async void OnServiceAdded(object sender, ServiceAnnouncementEventArgs e)
        {
            WLEDDevice toAdd = new WLEDDevice();
            foreach (var addr in e.Announcement.Addresses)
            {
                toAdd.NetworkAddress = addr.ToString(); break; //only get first address
            }
            toAdd.Name = e.Announcement.Hostname;
            toAdd.NameIsCustom = false;
            bool valid = await toAdd.Refresh();
            if (valid)
            {
                devicesFound++;
                OnValidDeviceFound(new DeviceCreatedEventArgs(toAdd, false));
            }
        }

        public static DeviceDiscovery GetInstance()
        {
            if (Instance == null) Instance = new DeviceDiscovery();
            return Instance;
        }

        protected virtual void OnValidDeviceFound(DeviceCreatedEventArgs e)
        {
            ValidDeviceFound?.Invoke(this, e);
        }

        protected virtual void OnDiscoveryEnd(DiscoveryResultEventArgs e)
        {
            DiscoveryResult?.Invoke(this, e);
        }
    }

    public class DiscoveryResultEventArgs
    {
        public bool WasSuccessful { get; }
        public string Message { get; }
        public int DevicesFoundAmount { get; }

        public DiscoveryResultEventArgs(bool success, string message, int deviceCount = 0)
        {
            WasSuccessful = success;
            Message = message;
            DevicesFoundAmount = deviceCount;
        }
    }
}
