using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace WLED
{
    class XmlApiResponseParser
    {
        public static string GetNameFromResponse(string xml)
        {
            string name = null;
            if (xml == null) return null;
            try
            {
                XElement resp = XElement.Parse(xml);
                name = resp.Element("ds")?.Value;
                if (name == null) name = resp.Element("desc")?.Value; //try legacy XML element name (pre WLED 0.6.0)
            } catch
            {

            }
            
            return name;
        }
    }
}
