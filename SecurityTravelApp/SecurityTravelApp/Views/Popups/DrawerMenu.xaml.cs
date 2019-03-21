using Rg.Plugins.Popup.Services;
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
    public partial class DrawerMenu : Rg.Plugins.Popup.Pages.PopupPage
    {
        public DrawerMenu()
        {
            InitializeComponent();

            // correct the behavior of backgroud tapping in android
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                var tapGestureRecognizerContainer = new TapGestureRecognizer();
                tapGestureRecognizerContainer.Tapped += async (s, e) =>
                {
                    await closeAppMenu();
                };
                container.GestureRecognizers.Add(tapGestureRecognizerContainer);
            }

            // add tap_handler for the backIcon button
            var tapGestureRecognizerBack = new TapGestureRecognizer();
            tapGestureRecognizerBack.Tapped += async (s, e) =>
            {
                await closeAppMenu();
            };
            backIcon.GestureRecognizers.Add(tapGestureRecognizerBack);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SideMenu.TranslateTo(0, 0, 400, Easing.CubicOut);
        }


        private async void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            await closeAppMenu();
        }

        private async Task<Boolean> closeAppMenu()
        {
            await SideMenu.TranslateTo(-250, 0, 400, Easing.CubicInOut);
            PopupNavigation.Instance.RemovePageAsync(this);
            return true;
        }

        protected override bool OnBackButtonPressed()
        {
            closeAppMenu();
            return base.OnBackButtonPressed();
        }
    }
}