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
    public enum ButtonIcon { None, Back, Add, Delete, Done }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuBar : ContentView
    {
        public event EventHandler LeftButtonTapped, RightButtonTapped;

        //custom menu bar present on all content pages
        public MenuBar()
        {
            InitializeComponent();
        }

        public void SetButtonIcon(ButtonLocation loc, ButtonIcon ico)
        {
            string path = "";

            switch (ico)
            {
                case ButtonIcon.Back: path = "icon_back.png"; break;
                case ButtonIcon.Add: path = "icon_add.png"; break;
                case ButtonIcon.Delete: path = "icon_bin.png"; break;
                case ButtonIcon.Done: path = "icon_check.png"; break;
            }

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
                Navigation.PopModalAsync(false);
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