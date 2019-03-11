using FFImageLoading.Svg.Forms;
using SecurityTravelApp.Services;
using SecurityTravelApp.ViewModels;
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
    public partial class HomePage : ContentPage, UpdatablePage
    {
        AppManagementService appMngSrv;
        CallService callSrv;
        private Animation mapMarkerAnimation;
        private double BounceElevation = 50;

        public HomePage(ServiceFactory pSrvFactory, AfterNavigationParams pParam)
        {
            InitializeComponent();
            SosSlider.initializeConfig(pSrvFactory);
            NavigationBar.initializeContent(pSrvFactory, pParam);
            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            callSrv = (CallService)pSrvFactory.getService(ServiceType.Call);

            // add tap gesture recognizer 
            var tapGestureRecognizerMapMarker = new TapGestureRecognizer();

            // setting the handler
            tapGestureRecognizerMapMarker.Tapped += async (s, e) =>
            {
                bounceAnimate(MapMarker, BounceElevation);
            };
            MapMarker.GestureRecognizers.Add(tapGestureRecognizerMapMarker);

            // add tap gesture to Hotline Button
            var tapGestureRecognizerHotline = new TapGestureRecognizer();

            // setting the handler
            tapGestureRecognizerHotline.Tapped += async (s, e) =>
            {
                callSrv.callNumber("+212611111111");
            };
            HotlineButton.GestureRecognizers.Add(tapGestureRecognizerHotline);
        }

        public void bounceAnimate(VisualElement pElement, double pElevation)
        {
            // create the bouncing animation
            mapMarkerAnimation = new Animation();
            var elevateAnimation = new Animation(d => pElement.TranslationY = d, 0, -pElevation, Easing.CubicOut);
            var bounceAnimation = new Animation(d => pElement.TranslationY = d, -pElevation, 0, Easing.BounceOut);
            mapMarkerAnimation.Add(0, .5, elevateAnimation);
            mapMarkerAnimation.Add(.5, 1, bounceAnimation);
            mapMarkerAnimation.Commit(MapMarker, "bounce", length: 1000);
        }

        public void update()
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await appMngSrv.checkForAllRequiredPermissions();
        }
    }
}