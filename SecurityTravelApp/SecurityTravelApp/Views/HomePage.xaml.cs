﻿using FFImageLoading.Svg.Forms;
using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
using SecurityTravelApp.ViewModels;
using SecurityTravelApp.Views.Popups;
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

            // getting values from xaml
            LastCheckinValueOpacity = Utilities.getOnPlatformValue<Double>(this.Resources["PositionInfoValueOpacity"]);

            // subscribe to location updates
            MessagingCenter.Subscribe<LocationService, Geoposition>(this, "LOCATIONUPDATE", (sender, pGeoposition) =>
            {
                gpsPositionAfterGet(pGeoposition);
                appMngSrv.incementNotif(NavigationItemTarget.Messages);
            });

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
                    locationSrv.getUserGeoposition();
                }
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
                PopupNavigation.Instance.PushAsync(new DrawerMenu(), false);
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
            continueWaitingAnimation = false;
            GpsWaitingIcon.IsVisible = false;
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


        public void update(NavigationParams pParam)
        {
            // update navigation bar
            NavigationBar.update(pParam);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await appMngSrv.checkForAllRequiredPermissions();
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