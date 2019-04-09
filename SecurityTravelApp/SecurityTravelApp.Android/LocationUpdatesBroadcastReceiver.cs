using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Support.V4.App;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;

namespace SecurityTravelApp.Droid
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { ACTION_PROCESS_UPDATES })]
    public class LocationUpdatesBroadcastReceiver : BroadcastReceiver
    {
        public const String ACTION_PROCESS_UPDATES = "TravelSecurity" + ".PROCESS_UPDATES";
        

        private LocationHelper_Droid locationHelper;
        private LocalNotifHelper_Droid localNotifHelper;

        public LocationUpdatesBroadcastReceiver()
        {
            locationHelper = new LocationHelper_Droid();
            localNotifHelper = new LocalNotifHelper_Droid();
        }

        public override void OnReceive(Context pContext, Intent intent)
        {
            try
            {
                if (intent != null)
                {
                    String action = intent.Action;
                    if (ACTION_PROCESS_UPDATES.Equals(action))
                    {
                        Location value = (Location)intent.Extras.Get("location");
                        Geoposition location = LocationHelper_Droid.toGeoposition(value);
                        localNotifHelper.notifyTrackingUpdate(location);

                        // launch job to save in database and send it to server when data transfer is active
                        //LocationUpdateJobService.EnqueueWork()
                    }
                }
            }
            catch (Exception e)
            {
                var locationHelper = new LocationHelper_Droid();
                locationHelper.disableBackgroundLocationUpdates();
            }

        }
    }
}