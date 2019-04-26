using Plugin.Geolocator;
using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
using SecurityTravelApp.ViewModels;
using SecurityTravelApp.Views.Popups;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage, Updatable, I18nable
    {
        AppManagementService appMngSrv;
        CallService callSrv;
        LocationService locationSrv;
        AudioService audioSrv;
        LocalDataService localDataSrv;
        LocalNotifService localNotifSrv;
        private double BounceElevation = 50;
        private Animation waitingAnimation;
        private Boolean continueWaitingAnimation = false;
        private double LastCheckinValueOpacity;
        private HomeViewModel viewModel;



        public HomePage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            viewModel = new HomeViewModel();
            BindingContext = viewModel;

            SosSlider.initializeConfig(pSrvFactory);
            NavigationBar.initializeContent(pSrvFactory, pParam);

            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            callSrv = (CallService)pSrvFactory.getService(ServiceType.Call);
            locationSrv = (LocationService)pSrvFactory.getService(ServiceType.Location);
            audioSrv = (AudioService)pSrvFactory.getService(ServiceType.Audio);
            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);
            localNotifSrv = (LocalNotifService)pSrvFactory.getService(ServiceType.LocalNotif);

            // getting values from xaml
            LastCheckinValueOpacity = Utilities.getOnPlatformValue<Double>(this.Resources["PositionInfoValueOpacity"]);


            // subscribe to location updates
            MessagingCenter.Subscribe<LocationService, Geoposition>(this, "LOCATIONUPDATE", (sender, pGeoposition) =>
            {
                gpsPositionAfterGet(pGeoposition);
                localDataSrv.savePosition(pGeoposition);
                // try send the location
            });

            // subscribe to SOS location updates
            MessagingCenter.Subscribe<LocationService, Geoposition>(this, "LOCATIONUPDATESOS", (sender, pGeoposition) =>
            {
                // just send the damn SOS location
            });


            // subscribe to Clear data event
            MessagingCenter.Subscribe<DrawerMenu>(this, "CLEAROLDDATA", (sender) =>
            {
                // display popup clear old data
                PopupNavigation.Instance.PushAsync(new DeleteOldDataPopup(pSrvFactory));
            });

            // subscribe to Lougout event
            MessagingCenter.Subscribe<DrawerMenu>(this, "USERLOGOUT", (sender) =>
            {
                // ensure pending actions will complete before loging out

                // save flag
                LocalDataService.setUserLoggedInFlag(false);
                appMngSrv.navigateTo(NavigationItemTarget.Login, pSrvFactory);
            });

            // subscribe to Lang Select FR
            MessagingCenter.Subscribe<DrawerMenu>(this, "FRLANGSELECT", (sender) =>
            {
                I18n.SelectLang(AppLanguage.FR);
                updateTXT();

                // save preference
                LocalDataService.setLanguagePreference(AppLanguage.FR);
            });

            // subscribe to Lang Select EN
            MessagingCenter.Subscribe<DrawerMenu>(this, "ENLANGSELECT", (sender) =>
            {
                I18n.SelectLang(AppLanguage.EN);
                updateTXT();

                // save preference
                LocalDataService.setLanguagePreference(AppLanguage.EN);
            });


            // subscribe to SOS Trigger
            MessagingCenter.Subscribe<TapSliderComp>(this, "SOS", (sender) =>
            {
                performSOSProcedure();
            });

            // add tap gesture recognizer 
            var tapGestureRecognizerMapMarker = new TapGestureRecognizer();

            // setting the handler
            tapGestureRecognizerMapMarker.Tapped += async (s, e) =>
            {
                waitingForGpsPosition();
                Utilities.bounceAnimate(MapMarker, BounceElevation);

                // perform SOS procedure 
                sendPosition();

                // hide the gps indication since the user knows how to send gps henceforth
                if (GPSIndication.Opacity > 0)
                {
                    GPSIndication.FadeTo(0, 4000);
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
                SideButton.IsEnabled = false;
                PopupNavigation.Instance.PushAsync(new DrawerMenu(), false);
                SideButton.IsEnabled = true;
            };

            // animation waiting for GPS
            waitingAnimation = new Animation();
            var rotateAnimation = new Animation(v => GpsWaitingIcon.Rotation = v, 0, 360);
            var fadeinAnimation = new Animation(v => GpsWaitingIcon.Opacity = v, 0, 1);
            var fadeoutAnimation = new Animation(v => GpsWaitingIcon.Opacity = v, 1, 0);

            waitingAnimation.Add(0, 1, rotateAnimation);
            waitingAnimation.Add(0, .2, fadeinAnimation);
            waitingAnimation.Add(.8, 1, fadeoutAnimation);

            // text
            updateTXT();

        }

        public void updateTXT()
        {
            WelcomeTXT.Text = I18n.GetText(AppTextID.WELCOME_GCC);
            LastCheckinLabel.Text = I18n.GetText(AppTextID.LAST_CHECKIN);
            GPSIndication.Text = I18n.GetText(AppTextID.GPS_TAP_INDICATION);
            NavigationBar.updateTXT();
        }

        public void sendPosition()
        {

            if (locationSrv.isGpsEnabled())
            {
                if (locationSrv.isUserBeingLocated())
                {
                    // do nothing, the current location is being retirived and will
                    // be sent as soon as it is available
                }
                else
                {
                    // start getting location
                    locationSrv.getUserGeoposition();
                    // sending to server is done upon updates arrival through subscription to LOCATIONUPDATE }
                }

            }
            else
            {
                cancelWaitingForGpsAnimation();
                DisplayAlert("Alert", I18n.GetText(AppTextID.ALERT_GPS_DISABLED), "OK");
            }
        }

        public void performSOSProcedure()
        {
            if (locationSrv.isGpsEnabled())
            {
                // start getting location SOS
                //locationSrv.getUserGeopositionSOS();
                // start audio recording
                //var audioFile = audioSrv.recordAudio();
                //if (audioFile != null)
                //{
                //    // add reference to file in database, later the audio will be sent


                //}
                // aaaaaaaand then make a call, yeah something s not right is this order but welp
                callSrv.callNumber("+212600000000");
            }
            else
            {
                DisplayAlert("Alert", I18n.GetText(AppTextID.ALERT_GPS_DISABLED), "OK");
            }
        }

        public void waitingForGpsPosition()
        {
            GpsWaitingIcon.Opacity = 0;
            GpsWaitingIcon.IsVisible = true;
            GpsCheckIcon.IsVisible = false;
            continueWaitingAnimation = true;
            waitingAnimation.Commit(this, "waiting", 16, 2000, null, repeat: () => { return continueWaitingAnimation; });
        }

        public void cancelWaitingForGpsAnimation()
        {
            ViewExtensions.CancelAnimations(GpsWaitingIcon);
            continueWaitingAnimation = false;
            GpsWaitingIcon.IsVisible = false;
        }

        public async void gpsPositioningDone()
        {
            cancelWaitingForGpsAnimation();
            GpsCheckIcon.Opacity = 0;
            GpsCheckIcon.IsVisible = true;
            await GpsCheckIcon.FadeTo(1, 200);
            GpsCheckIcon.FadeTo(0, 2000);
        }

        public async void gpsPositionAfterGet(Geoposition pGeoposition)
        {
            // check in map marker
            gpsPositioningDone();

            // update location values
            viewModel.geoposition = pGeoposition;

            // effect updating 
            await LastCheckinLabel.FadeTo(0, 80);
            LastCheckinLabel.FadeTo(LastCheckinValueOpacity, 80);

            // update data base




        }

        public void continueTrackingForeGround()
        {
            // check if the user should continue being tracked
            LocalDataService.setUserTrackingFlag(true);

            // continue tracking if yes
            if (LocalDataService.getUserTrackingFlag())
            {
                // notify user
                localNotifSrv.showNotifTrackingOn();


                // launch geoposition updates in foreground whether  the GPS is On or Off
                locationSrv.disableBackgroundLocationUpdates();
                locationSrv.trackUserGeopositionInForeGround();

                // and then ask to activate GPS if it is Off 
                if (!locationSrv.isGpsEnabled())
                {
                    DisplayAlert("Alert", I18n.GetText(AppTextID.ALERT_GPS_DISABLED), "OK");
                }

            }
        }


        public void update(NavigationParams pParam)
        {
            // update navigation bar
            NavigationBar.update(pParam);
            // update data
            if (!pParam.NavigationBarOnly)
            {

            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await appMngSrv.checkForAllRequiredPermissions();

            if (LocalDataService.getUserTrackingFlag())
            {
                // user was being tracked while the app is background
                continueTrackingForeGround();
            }

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

    }
}