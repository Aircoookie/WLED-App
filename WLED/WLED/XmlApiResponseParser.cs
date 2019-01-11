using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Xamarin.Forms;

namespace WLED
{
    class XmlApiResponseParser
    {
        public static XmlApiResponse ParseApiResponse(string xml)
        {
            XmlApiResponse resp = new XmlApiResponse();
            if (xml == null) return resp;
            try
            {
                XElement xe = XElement.Parse(xml);
                if (xe == null) return null;
                resp.Name = xe.Element("ds")?.Value;
                if (resp.Name == null) resp.Name = xe.Element("desc")?.Value; //try legacy XML element name (pre WLED 0.6.0)
                if (resp.Name == null) return null; //if we return at this point, parsing was unsuccessful (server likely not WLED device)

                string bri_s = xe.Element("ac")?.Value;
                if (bri_s == null) bri_s = xe.Element("act")?.Value; //try legacy XML element name (pre WLED 0.6.0)
                if (bri_s != null)
                {
                    int bri = 0;
                    Int32.TryParse(bri_s, out bri);
                    if (bri > 0) resp.Brightness = (byte)bri; //keep stored brightness if brightness == 0
                    resp.State = (bri > 0); //light is on if brightness > 0
                }

                double r = 0, g = 0, b = 0;
                int counter = 0;
                foreach (var el in xe.Elements("cl"))
                {
                    int co = 0;
                    Int32.TryParse(el?.Value, out co);
                    switch (counter)
                    {
                        case 0: r = co / 255; break;
                        case 1: g = co / 255; break;
                        case 2: b = co / 255; break;
                    }
                }
                resp.LightColor = new Color(r, g, b);
            } catch
            {
                //Exceptions here indicate unsuccessful parsing and may thus be ignored
            }
            return resp;
        }
    }
}
