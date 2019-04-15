using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotifAlertPopup : Rg.Plugins.Popup.Pages.PopupPage, I18nable
    {
        public NotifAlertPopup()
        {
            InitializeComponent();


            var flashLoop = new Animation();
            Animation fadeIn1 = new Animation(d => Siren1Light.Opacity = d, 0, 1, Easing.CubicIn);
            Animation fadeOut1 = new Animation(d => Siren1Light.Opacity = d, 1, 0, Easing.CubicOut);
            Animation fadeIn2 = new Animation(d => Siren2Light.Opacity = d, 0, 1, Easing.CubicIn);
            Animation fadeOut2 = new Animation(d => Siren2Light.Opacity = d, 1, 0, Easing.CubicOut);
            flashLoop.Add(.1, .5, fadeIn1);
            flashLoop.Add(.5, .9, fadeOut1);
            flashLoop.Add(0, .4, fadeOut2);
            flashLoop.Add(.6, 1, fadeIn2);

            flashLoop.Commit(this, "flash", length: 1000, repeat: () => { return true; });

        }

        public void updateTXT()
        {
        }
    }
}