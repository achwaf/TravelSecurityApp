using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Droid;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationHelper_Droid))]
namespace SecurityTravelApp.Droid
{
    class LocationHelper_Droid : Java.Lang.Object, ILocationHelper, ILocationListener
    {

        public event EventHandler LocationChanged;


        LocationManager locationManager;
        Location location;

        public LocationHelper_Droid()
        {
            locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
        }

        public Geoposition getLastKnownGPSLocation()
        {
            location = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
            return toGeoposition(location);
        }

        public Geoposition getLastKnownNetworkLocation()
        {
            location = locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);
            return toGeoposition(location);
        }

        public void reactivateLocationUpdates(int IntervallTime)
        {
            locationManager.RemoveUpdates(this);
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, IntervallTime, 0, this);
            locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, IntervallTime, 0, this);
        }

        private PendingIntent getPendingIntent()
        {
            Context context = Android.App.Application.Context;
            Intent intent = new Intent(context, typeof(LocationUpdatesBroadcastReceiver));
            intent.SetAction(LocationUpdatesBroadcastReceiver.ACTION_PROCESS_UPDATES);
            return PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
        }

        public void activateBackgroundLocationUpdates(int IntervallTime)
        {
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, IntervallTime, 0, getPendingIntent());
            locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, IntervallTime, 0, getPendingIntent());
        }


        public void disableBackgroundLocationUpdates()
        {
            locationManager.RemoveUpdates(getPendingIntent());
        }

        public void disableLocationUpdates()
        {
            locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            LocationChanged?.Invoke(this, new GeopositionEventArgs(toGeoposition(location)));
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }

        public void Dispose()
        {
        }

        public static Geoposition toGeoposition(Location pLocation)
        {

            if (pLocation != null)
            {
                Geoposition geoposition = new Geoposition();
                geoposition.HasAccuracy = pLocation.HasAccuracy;
                geoposition.Accuracy = pLocation.Accuracy;
                geoposition.Altitude = pLocation.Altitude;
                geoposition.Longitude = pLocation.Longitude;
                geoposition.Latitude = pLocation.Latitude;
                geoposition.Provider = pLocation.Provider;
                geoposition.Time = pLocation.Time;
                geoposition.Date = DateTime.Now;
                return geoposition;
            }
            else return null;

        }
    }
}