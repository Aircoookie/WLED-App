using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WLED
{
    class DeviceHTTPConnection
    {
        private static HttpClient Client = new HttpClient();

        public static async Task<string> Send_WLED_API_Call(string DeviceURI, string API_Call)
        {
            try
            {
                var result = await Client.GetAsync(DeviceURI + "/win&" + API_Call);
                return await result.Content.ReadAsStringAsync();
            } catch
            {
                return "Network Error";
            }
        }
    }
}
