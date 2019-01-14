using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WLED
{
    class DeviceHttpConnection
    {
        private static DeviceHttpConnection Instance;

        private HttpClient Client;

        private DeviceHttpConnection ()
        {
            Client = new HttpClient();
            Client.Timeout = TimeSpan.FromSeconds(5);
        }
        
        public static DeviceHttpConnection GetInstance()
        {
            if (Instance == null) Instance = new DeviceHttpConnection();
            return Instance;
        }

        public async Task<string> Send_WLED_API_Call(string DeviceURI, string API_Call)
        {
            try
            {
                string apiCommand = "/win"; //WLED http API URI
                if (API_Call != null && API_Call.Length > 0)
                {
                    apiCommand += "&";
                    apiCommand += API_Call;
                }
                var result = await Client.GetAsync(DeviceURI + apiCommand);
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
                } else //404 or other non-success status codes, indicates that target is not WLED device
                {
                    return "err";
                }
            } catch
            {
                return null; //time-out or other connection error
            }
        }
    }
}
