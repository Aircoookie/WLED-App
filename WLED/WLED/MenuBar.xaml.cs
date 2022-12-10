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

    //View Element: Custom menu bar present on all content pages
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuBar : ContentView
    {
        public event EventHandler LeftButtonTapped, RightButtonTapped;

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

        void OnLogoTapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://github.com/Aircoookie/WLED"));
        }

        protected virtual void OnLeftButtonTapped(object sender, EventArgs e)
        {
            EventHandler handler = LeftButtonTapped;
            if (handler != null)
            {
                handler(this, e);
            } else
            {
                //The default left button behavior is a back button (if the parent view doesn't attach a custom handler)
                Navigation.PopModalAsync(false);
            }
        }

        protected virtual void OnRightButtonTapped(object sender, EventArgs e)
        {
            RightButtonTapped?.Invoke(this, e);
        }
    }
}