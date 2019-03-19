using FFImageLoading.Svg.Forms;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
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
        LocationService locationSrv;
        private double BounceElevation = 50;
        private Animation waitingAnimation;
        private Boolean continueWaitingAnimation = false;


        public HomePage(ServiceFactory pSrvFactory, AfterNavigationParams pParam)
        {
            InitializeComponent();
            SosSlider.initializeConfig(pSrvFactory);
            NavigationBar.initializeContent(pSrvFactory, pParam);
            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            callSrv = (CallService)pSrvFactory.getService(ServiceType.Call);
            locationSrv = (LocationService)pSrvFactory.getService(ServiceType.Location);

            // add tap gesture recognizer 
            var tapGestureRecognizerMapMarker = new TapGestureRecognizer();

            // setting the handler
            tapGestureRecognizerMapMarker.Tapped += async (s, e) =>
            {
                waitingForGpsPosition();
                Utilities.bounceAnimate(MapMarker, BounceElevation);
                if (locationSrv.isUserBeingLocated())
                {
                    // do nothing
                }
                else
                {
                    //await Task.Delay(5000);
                    //gpsPositioningDone();
                }
            };
            MapMarker.GestureRecognizers.Add(tapGestureRecognizerMapMarker);

            // add tap gesture to Hotline Button
            var tapGestureRecognizerHotline = new TapGestureRecognizer();
            HotlineButton.GestureRecognizers.Add(tapGestureRecognizerHotline);

            // add tap gesture to Side Button
            var tapGestureRecognizerSideButton = new TapGestureRecognizer();
            SideButton.GestureRecognizers.Add(tapGestureRecognizerSideButton);

            // setting the handler to hotline Buton
            tapGestureRecognizerHotline.Tapped += (s, e) =>
            {
                callSrv.callNumber("+212611111111");
            };


            // setting the handler to Side  Buton
            tapGestureRecognizerSideButton.Tapped += (s, e) =>
            {
                gpsPositioningDone();
            };

            // animation waiting for GPS
            waitingAnimation = new Animation();
            var rotateAnimation = new Animation(v => GpsWaitingIcon.Rotation = v, 0, 360);
            var fadeinAnimation = new Animation(v => GpsWaitingIcon.Opacity = v, 0, 1);
            var fadeoutAnimation = new Animation(v => GpsWaitingIcon.Opacity = v, 1, 0);

            waitingAnimation.Add(0, 1, rotateAnimation);
            waitingAnimation.Add(0, .2, fadeinAnimation);
            waitingAnimation.Add(.8, 1, fadeoutAnimation);

        }

        public void waitingForGpsPosition()
        {
            GpsWaitingIcon.Opacity = 0;
            GpsWaitingIcon.IsVisible = true;
            GpsCheckIcon.IsVisible = false;
            continueWaitingAnimation = true;
            waitingAnimation.Commit(this, "waiting", 16, 2000, null, repeat: () => { return continueWaitingAnimation; });
        }

        public async void gpsPositioningDone()
        {
            ViewExtensions.CancelAnimations(GpsWaitingIcon);
            //continueWaitingAnimation = false;
            GpsWaitingIcon.IsVisible = false;
            GpsCheckIcon.Opacity = 0;
            GpsCheckIcon.IsVisible = true;
            await GpsCheckIcon.FadeTo(1, 200);
            GpsCheckIcon.FadeTo(0, 2000);
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