﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
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
    class LocationHelper_Droid : Java.Lang.Object, LocationHelper, ILocationListener
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

        public void activateLocationUpdates(int IntervallTime)
        {
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, IntervallTime, 0, this);
            locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, IntervallTime, 0, this);
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

        public Geoposition toGeoposition(Location pLocation)
        {
            Geoposition geoposition = new Geoposition();

            if (pLocation != null)
            {
                geoposition.HasAccuracy = pLocation.HasAccuracy;
                geoposition.Accuracy = pLocation.Accuracy;
                geoposition.Altitude = pLocation.Altitude;
                geoposition.Longitude = pLocation.Longitude;
                geoposition.Latitude = pLocation.Latitude;
                geoposition.Provider = pLocation.Provider;
                geoposition.Time = pLocation.Time;
            }
            return geoposition;

        }
    }
}