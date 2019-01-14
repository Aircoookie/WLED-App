using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WLED
{
    //data object containing values from WLED API response
    class XmlApiResponse
    {
        public byte Brightness { get; set; } = 128;
        public bool IsOn { get; set; } = false;
        public Color LightColor { get; set; }
        public string Name { get; set; } = "";
    }
}
