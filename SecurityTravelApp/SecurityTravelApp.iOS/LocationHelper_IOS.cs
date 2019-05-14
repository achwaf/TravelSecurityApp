using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.iOS;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationHelper_IOS))]
namespace SecurityTravelApp.iOS
{
    class LocationHelper_IOS : ILocationHelper
    {
        public event EventHandler LocationChanged;

        CLLocationManager locationManager;
        CLLocation location;

        public LocationHelper_IOS()
        {
            locationManager = new CLLocationManager();
            locationManager.PausesLocationUpdatesAutomatically = false;


            // set the listener
            locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
            {
                // fire our custom Location Updated event
                LocationChanged?.Invoke(this, new GeopositionEventArgs(toGeoposition(e.Locations[e.Locations.Length - 1])));
            };

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locationManager.RequestAlwaysAuthorization(); // works in background
                //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locationManager.AllowsBackgroundLocationUpdates = true;
            }
        }


        public void activateBackgroundLocationUpdates(int IntervalTime)
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                //set the desired frequency
                locationManager.DistanceFilter = CLLocationDistance.FilterNone;
                locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
                locationManager.AllowDeferredLocationUpdatesUntil(999999, IntervalTime / 1000);
                locationManager.StartUpdatingLocation();
            }
        }

        public void disableBackgroundLocationUpdates()
        {
            throw new NotImplementedException();
        }

        public void disableLocationUpdates()
        {
            throw new NotImplementedException();
        }

        public Geoposition getLastKnownGPSLocation()
        {
            return toGeoposition(locationManager.Location);
        }

        public Geoposition getLastKnownNetworkLocation()
        {
            return toGeoposition(locationManager.Location);
        }

        public void reactivateLocationUpdates(int IntervalTime)
        {
            throw new NotImplementedException();
        }

        public static Geoposition toGeoposition(CLLocation pLocation)
        {

            if (pLocation != null)
            {
                Geoposition geoposition = new Geoposition();
                geoposition.Accuracy = (float)pLocation.HorizontalAccuracy;
                geoposition.Altitude = pLocation.Altitude;
                geoposition.Longitude = pLocation.Coordinate.Longitude;
                geoposition.Latitude = pLocation.Coordinate.Latitude;
                geoposition.Time = (long)(pLocation.Timestamp.SecondsSinceReferenceDate * 1000); // get in milliseconds
                geoposition.Date = DateTime.Now;
                return geoposition;
            }
            else return null;

        }
    }
}