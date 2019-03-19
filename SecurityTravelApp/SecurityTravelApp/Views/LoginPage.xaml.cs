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

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage, UpdatablePage
    {

        AppManagementService appMngSrv;

        public LoginPage(ServiceFactory pSrvFactory, AfterNavigationParams pParam)
        {
            InitializeComponent();
            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);

            // setting the taphandler for togglepass   
            var tapGestureRecognizerPass = new TapGestureRecognizer();
            TogglePass.GestureRecognizers.Add(tapGestureRecognizerPass);


            // setting the taphandler for login button  
            var tapGestureRecognizerLogin = new TapGestureRecognizer();
            LoginButton.GestureRecognizers.Add(tapGestureRecognizerLogin);

            // defining tap handler Pass
            tapGestureRecognizerPass.Tapped += (s, e) =>
            {
                if (ShowPass.IsVisible)
                {
                    PassEntry.IsPassword = true;
                    Utilities.switchBetween(ShowPass, HidePass);
                }
                else if (HidePass.IsVisible)
                {
                    PassEntry.IsPassword = false;
                    Utilities.switchBetween(HidePass, ShowPass);
                }
            };

            // defining tap handler Login
            tapGestureRecognizerLogin.Tapped += (s, e) =>
            {
                appMngSrv.navigateToAndSave(appMngSrv.lookUpPage(NavigationItemTarget.Home), NavigationItemTarget.Home);
            };
        }

        private async void welcomeAniamtion()
        {
            await animateLogo();
            await Task.Delay(1000);
            showLoginEntries();
        }


        private async Task animateLogo()
        {
            Logo1.Opacity = 1;
            Logo2.Opacity = 0;
            Logo1.IsVisible = true;
            Logo2.IsVisible = true;

            await Logo1.FadeTo(0, 1500, Easing.CubicIn);
            Logo2.FadeTo(1, 3000, Easing.CubicOut);

        }

        private async void showLoginEntries()
        {
            LoginComp.TranslationY = 40;
            LoginComp.Opacity = 0;
            LoginComp.FadeTo(1, 200, Easing.CubicIn);
            await LoginComp.TranslateTo(0, 0, 200, Easing.CubicIn);
        }


        public void update()
        {
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            // UI init
            LoginComp.Opacity = 0;
            welcomeAniamtion();
        }
    }
}