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

            topMenuBar.SetButtonIcon(ButtonLocation.Left, "icon_back.png");
            topMenuBar.LeftButtonTapped += On_BackButton_Tapped;
        }

        async void On_BackButton_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}