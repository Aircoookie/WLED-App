using System;
using System.Collections.Generic;
using System.Text;

namespace WLED
{
    class NetUtility
    {
        public static string PrependHTTPToURL(string baseURL)
        {
            if (baseURL == null) return null;

            if (!baseURL.StartsWith("http://")) baseURL = "http://" + baseURL;

            if (baseURL.Length > 7)
            {
                return baseURL;
            }
            return null;
        }
    }
}
