using Plugin.Geolocator;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SecurityTravelApp.Services
{
    public class LocationService : ServiceAbstract
    {

        const ServiceType TYPE = ServiceType.Location;
        private const int STALETIME = 1000 * 60 * 2; // 120 seconds
        private const int INTERVALTIME = 1000 * 4;
        private const int INTERVALTIMEFIRSTGET = 0;
        private const int TIMEVALIDITY = 1000 * 30;
        private const int GETLOCATIONATTEMPTS = 3;

        private ILocationHelper locationHelper;
        private Geoposition currentGeoposition;
        private int locationAttemptsCounter;
        private bool userIsBeingLocated;
        private bool userIsBeingTracked;
        private bool isSOSActivted = false;

        public LocationService() : base(TYPE)
        {
            locationHelper = DependencyService.Get<ILocationHelper>();
            locationHelper.LocationChanged += locationHelper_LocationChanged;
        }

        public Boolean isUserBeingLocated()
        {
            return userIsBeingLocated;
        }


        public Boolean isUserBeingTracked()
        {
            return userIsBeingTracked;
        }

        public void getUserGeoposition()
        {

            Debug.WriteLine("GPS requests Launched");
            if (!userIsBeingLocated)
            {
                userIsBeingLocated = true;
                // get the last known geoposition
                Geoposition gpsPosition = locationHelper.getLastKnownGPSLocation();
                Geoposition networkPosition = locationHelper.getLastKnownNetworkLocation();
                if (networkPosition != null)
                {
                    currentGeoposition = betterLocation(networkPosition, gpsPosition);
                }
                else
                {
                    currentGeoposition = betterLocation(gpsPosition, networkPosition);
                }

                // activate listening to providers
                locationAttemptsCounter = GETLOCATIONATTEMPTS;
                locationHelper.reactivateLocationUpdates(INTERVALTIMEFIRSTGET);

            }
        }

        public void trackUserGeopositionInBackground()
        {
            // this method is supposed to be called only when the application goes to background
            if (!userIsBeingTracked)
            {
                // IMPORTANT 
                // the code of this class is not involved in these background updates
                // we only activate the updates
                // the executed code is in the BroadReciever Android, IOS is not yet implemented
                userIsBeingLocated = true;
                userIsBeingTracked = true;
                locationHelper.disableLocationUpdates();
                locationHelper.activateBackgroundLocationUpdates(INTERVALTIME);
            }
        }

        public void trackUserGeopositionInForeGround()
        {
            if (!userIsBeingTracked)
            {
                userIsBeingLocated = true;
                userIsBeingTracked = true;
                locationHelper.reactivateLocationUpdates(INTERVALTIME);
            }
        }

        public void stopTrackingUserGeoposition()
        {
            userIsBeingTracked = false;
        }

        public void getUserGeopositionSOS()
        {
            isSOSActivted = true;
            getUserGeoposition();
            locationAttemptsCounter = GETLOCATIONATTEMPTS;
        }

        public void disableBackgroundLocationUpdates()
        {
            locationHelper.disableBackgroundLocationUpdates();
        }

        public Boolean isGpsEnabled()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            return CrossGeolocator.Current.IsGeolocationEnabled;
        }


        private void locationHelper_LocationChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Location updated " + locationAttemptsCounter);
            // listen to N attempts for better accuracy
            Geoposition newPosition = ((GeopositionEventArgs)e).Position;

            Debug.Write(" Location is  " + newPosition.Latitude + ":" + newPosition.Longitude + "/" + newPosition.Accuracy);
            if (userIsBeingLocated)
            {
                if (--locationAttemptsCounter > 0)
                {
                    if (betterLocation(newPosition, currentGeoposition) != currentGeoposition)
                    {
                        currentGeoposition = newPosition;
                    }

                    // send all attempts in case of SOS update
                    if (isSOSActivted)
                    {
                        currentGeoposition.IsSOS = true;
                        MessagingCenter.Send<LocationService, Geoposition>(this, "LOCATIONUPDATESOS", currentGeoposition);
                    }
                    else
                    {
                        //test
                        MessagingCenter.Send<LocationService, Geoposition>(this, "LOCATIONUPDATE", currentGeoposition);
                    }
                }
                else
                {
                    // IMPORTANT
                    // if the use is tracked , this bloc is overstepped and the loop goes on and on
                    if (!userIsBeingTracked)
                    {
                        // stop linstening
                        locationHelper.disableLocationUpdates();
                    }

                    // update the current position
                    if (betterLocation(newPosition, currentGeoposition) != currentGeoposition)
                    {
                        currentGeoposition = newPosition;
                    }
                    // send this last update as follow up to previous ones 
                    if (isSOSActivted)
                    {
                        currentGeoposition.IsSOS = true;
                        MessagingCenter.Send<LocationService, Geoposition>(this, "LOCATIONUPDATESOS", currentGeoposition);
                    }
                    // in non SOS case, this last location is normally the only one sent
                    else
                    {
                        currentGeoposition.IsSOS = false;
                        MessagingCenter.Send<LocationService, Geoposition>(this, "LOCATIONUPDATE", currentGeoposition);
                    }
                    // work is done

                    if (!userIsBeingTracked)
                    {
                        userIsBeingLocated = false;
                    }
                    isSOSActivted = false;
                }
            }
            // if other updates have arrived after the user has been located, update the currentGeoposition
            else
            {
                if (betterLocation(newPosition, currentGeoposition) != currentGeoposition)
                {
                    currentGeoposition = newPosition;
                }
            }

        }



        private Geoposition betterLocation(Geoposition pLocation1, Geoposition pLocation2)
        {
            if (pLocation2 == null)
            {
                // A new location is always better than no location
                return pLocation1;
            }

            // Check whether the new location fix is newer or older
            long timeDelta = pLocation1.Time - pLocation2.Time;
            bool isSignificantlyNewer = timeDelta > STALETIME;
            bool isSignificantlyOlder = timeDelta < -STALETIME;
            bool isNewer = timeDelta > 0;

            // If it's been more than two minutes since the current location, use the new location
            // because the user has likely moved
            if (isSignificantlyNewer)
            {
                return pLocation1;
                // If the new location is more than two minutes older, it must be worse
            }
            else if (isSignificantlyOlder)
            {
                return pLocation2;
            }

            // Check whether the new location fix is more or less accurate
            int accuracyDelta = (int)(pLocation1.Accuracy - pLocation2.Accuracy);
            bool isLessAccurate = accuracyDelta > 0;
            bool isMoreAccurate = accuracyDelta < 0;
            bool isSignificantlyLessAccurate = accuracyDelta > 200;

            // Check if the old and new location are from the same provider
            bool isFromSameProvider = isSameProvider(pLocation1.Provider, pLocation2.Provider);

            // Determine location quality using a combination of timeliness and accuracy
            if (isMoreAccurate)
            {
                return pLocation1;
            }
            else if (isNewer && !isLessAccurate)
            {
                return pLocation1;
            }
            else if (isNewer && !isSignificantlyLessAccurate && isFromSameProvider)
            {
                return pLocation1;
            }
            return pLocation2;
        }

        /** Checks whether two providers are the same */
        private bool isSameProvider(String provider1, String provider2)
        {
            if (provider1 == null)
            {
                return provider2 == null;
            }
            return provider1.Equals(provider2);
        }




    }

    public class GeopositionEventArgs : EventArgs
    {
        public Geoposition Position { get; set; }

        public GeopositionEventArgs(Geoposition pGeoposition)
        {
            Position = pGeoposition;
        }
    }
}
