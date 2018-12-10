using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WLED
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceControlPage : ContentPage
	{
		public DeviceControlPage (string pageURL)
		{
			InitializeComponent ();
            UIBrowser.Source = pageURL;
        }
	}
}