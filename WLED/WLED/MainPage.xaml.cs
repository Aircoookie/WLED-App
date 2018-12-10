using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WLED
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Start_Clicked(object sender, System.EventArgs e)
        {
            var page = new DeviceListViewPage();
            await Navigation.PushModalAsync(page);
        }
    }
}
