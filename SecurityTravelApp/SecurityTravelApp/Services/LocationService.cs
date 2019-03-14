using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SecurityTravelApp.Services
{
    public class LocationService : ServiceAbstract
    {

        const ServiceType TYPE = ServiceType.Location;
        private const int STALETIME = 1000 * 60 * 2;
        private const int INTERVALTIME = 1000 * 60 * 10;
        private const int TIMEVALIDITY = 1000 * 30;
        private const int GETLOCATIONATTEMPTS = 6;

        private LocationHelper locationHelper;
        private Geoposition currentGeoposition;
        private int locationAttemptsCounter;
        private bool userIsBeingLocated;
        Command commandAfterGettingLocation;

        public LocationService() : base(TYPE)
        {
            locationHelper = DependencyService.Get<LocationHelper>();
            locationHelper.LocationChanged += locationHelper_LocationChanged;
        }

        public void setUpdateCommand(Command pCallBackUponLocation)
        {
            commandAfterGettingLocation = pCallBackUponLocation;
        }

        public Boolean isUserBeingLocated()
        {
            return userIsBeingLocated;
        }

        public void getUserGeoposition()
        {
            if (!userIsBeingLocated)
            {
                userIsBeingLocated = true;
                // get the last known geoposition
                Geoposition gpsPosition = locationHelper.getLastKnownGPSLocation();
                Geoposition networkPosition = locationHelper.getLastKnownNetworkLocation();
                currentGeoposition = betterLocation(gpsPosition, networkPosition);

                // activate listening to providers
                locationAttemptsCounter = GETLOCATIONATTEMPTS;
                locationHelper.activateLocationUpdates(INTERVALTIME);

            }
        }


        private void locationHelper_LocationChanged(object sender, EventArgs e)
        {
            // listen to N attempts for better accuracy

            Geoposition newPosition = ((GeopositionEventArgs)e).Position;
            if (userIsBeingLocated)
            {
                if (--locationAttemptsCounter > 0)
                {
                    if (betterLocation(newPosition, currentGeoposition) != currentGeoposition)
                    {
                        currentGeoposition = newPosition;
                    }
                }
                else
                {
                    // stop linstening
                    locationHelper.disableLocationUpdates();
                    // update the current position
                    if (betterLocation(newPosition, currentGeoposition) != currentGeoposition)
                    {
                        currentGeoposition = newPosition;
                    }
                    // perform action
                    commandAfterGettingLocation.Execute(currentGeoposition);
                    // work is done
                    userIsBeingLocated = false;
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
