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
                var result = await Client.GetAsync(DeviceURI + "/win&" + API_Call);
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
                } else
                {
                    return "err";
                }
            } catch
            {
                return null;
            }
        }
    }
}
