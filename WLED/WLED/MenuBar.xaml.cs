using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WLED
{
    public enum ButtonLocation { Left, Right }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuBar : ContentView
    {
        public event EventHandler LeftButtonTapped, RightButtonTapped;

        public MenuBar()
        {
            InitializeComponent();
        }

        public void SetButtonIcon(ButtonLocation loc, string path)
        {
            if (loc == ButtonLocation.Left)
            {
                imageLeft.Source = path;
            } else
            {
                imageRight.Source = path;
            }
        }

        void On_Logo_Tapped(object sender, ItemTappedEventArgs e)
        {
            Device.OpenUri(new Uri("https://github.com/Aircoookie/WLED"));
        }

        protected virtual void On_LeftButton_Tapped(object sender, ItemTappedEventArgs e)
        {
            EventHandler handler = LeftButtonTapped;
            if (handler != null)
            {
                handler(this, e);
            } else
            {
                Navigation.PopModalAsync();
            }
        }

        protected virtual void On_RightButton_Tapped(object sender, ItemTappedEventArgs e)
        {
            EventHandler handler = RightButtonTapped;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}