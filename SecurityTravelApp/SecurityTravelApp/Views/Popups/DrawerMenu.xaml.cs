using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
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
    public partial class DrawerMenu : Rg.Plugins.Popup.Pages.PopupPage, I18nable
    {
        private AppLanguage SelectedLang;
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
                BackgroundContainer.GestureRecognizers.Add(tapGestureRecognizerContainer);
            }

            // add tap_handler for the backIcon button
            var tapGestureRecognizerBack = new TapGestureRecognizer();
            tapGestureRecognizerBack.Tapped += async (s, e) =>
            {
                await closeAppMenu();
            };
            backIcon.GestureRecognizers.Add(tapGestureRecognizerBack);

            // add tap_handler to logout action
            var tapGestureRecognizerLogout = new TapGestureRecognizer();
            tapGestureRecognizerLogout.Tapped += async (s, e) =>
            {
                await closeAppMenu();
                MessagingCenter.Send<DrawerMenu>(this, "USERLOGOUT");
            };
            LogoutAction.GestureRecognizers.Add(tapGestureRecognizerLogout);

            // add tap_handler to FR action
            var tapGestureRecognizerSelectFR = new TapGestureRecognizer();
            tapGestureRecognizerSelectFR.Tapped += async (s, e) =>
            {
                SelectedLang = AppLanguage.FR;
                MessagingCenter.Send<DrawerMenu>(this, "FRLANGSELECT");
                updateTXT();
            };
            FRLang.GestureRecognizers.Add(tapGestureRecognizerSelectFR);

            // add tap_handler to EN action
            var tapGestureRecognizerSelectEN = new TapGestureRecognizer();
            tapGestureRecognizerSelectEN.Tapped += async (s, e) =>
            {
                SelectedLang = AppLanguage.EN;
                MessagingCenter.Send<DrawerMenu>(this, "ENLANGSELECT");
                updateTXT();
            };
            ENLang.GestureRecognizers.Add(tapGestureRecognizerSelectEN);

            // apply language
            updateTXT();

        }

        public void updateTXT()
        {
            LogoutTXT.Text = I18n.GetText(AppTextID.LOGOUT, SelectedLang);
            RecordedTXT.Text = I18n.GetText(AppTextID.RECORDED_AUDIO, SelectedLang);
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
            return true;
        }
    }
}