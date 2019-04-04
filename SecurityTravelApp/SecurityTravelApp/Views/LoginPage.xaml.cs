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
    public partial class LoginPage : ContentPage, Updatable, I18nable
    {

        AppManagementService appMngSrv;
        LocalDataService localDataSrv;

        public LoginPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);

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
                LocalDataService.setUserLoggedInFlag(true);
                appMngSrv.navigateTo(NavigationItemTarget.Home, pSrvFactory, null);
            };

            // Text
            updateTXT();

        }


        public void updateTXT()
        {
            welcomeTXT.Text = I18n.GetText(AppTextID.WELCOME_GCC);
            PassEntry.Placeholder = I18n.GetText(AppTextID.PASSWORD);
            LoginTXT.Text = I18n.GetText(AppTextID.LOGIN);
        }


        private async Task welcomeAniamtion()
        {
            LoginComp.Opacity = 0;
            Logo1.Opacity = 1;
            Logo2.Opacity = 0;
            TravelLine.Opacity = 0;
            Logo1.IsVisible = true;
            Logo2.IsVisible = true;
            TravelLine.IsVisible = true;

            await Logo1.FadeTo(0, 1500, Easing.CubicIn);
            showLoginEntries();
            Logo2.FadeTo(1, 1500, Easing.CubicOut);
            TravelLine.FadeTo(.3, 1500, Easing.CubicOut);
            Logo1.IsVisible = false;
        }

        private async void showLoginEntries()
        {
            LoginComp.Opacity = 0;
            LoginComp.FadeTo(1, 1500, Easing.CubicOut);
        }


        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            // UI init
            LoginComp.Opacity = 0;
            welcomeAniamtion();
        }

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            //if (PopupNavigation.Instance.PopupStack.Count > 0)
            //{
            //    PopupNavigation.Instance.PopAsync();
            //}
            return true;
        }

        public void update(NavigationParams pParam)
        {
        }
    }

}