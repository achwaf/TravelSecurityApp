using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Support.V4.App;
using SecurityTravelApp.Models;

namespace SecurityTravelApp.Droid
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { ACTION_PROCESS_UPDATES })]
    public class LocationUpdatesBroadcastReceiver : BroadcastReceiver
    {
        public const String ACTION_PROCESS_UPDATES = "TravelSecurity" + ".PROCESS_UPDATES";
        private static String PRIMARY_CHANNEL = "default";

        private NotificationManager mNotificationManager;

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
                        showNotification(pContext, location);

                        // stop updates
                        var locationHelper = new LocationHelper_Droid();
                        locationHelper.disableLocationUpdates();
                    }
                }
            }
            catch (Exception e)
            {
                var locationHelper = new LocationHelper_Droid();
                locationHelper.disableLocationUpdates();
            }

        }


        void showNotification(Context pContext, Geoposition pLocation)
        {
            Intent notificationIntent = new Intent(pContext, typeof(MainActivity));

            // Construct a task stack.
            Android.App.TaskStackBuilder stackBuilder = Android.App.TaskStackBuilder.Create(pContext);

            // Add the main Activity to the task stack as the parent.
            stackBuilder.AddParentStack(Java.Lang.Class.FromType((typeof(MainActivity))));

            // Push the content Intent onto the stack.
            stackBuilder.AddNextIntent(notificationIntent);

            // Get a PendingIntent containing the entire back stack.
            PendingIntent notificationPendingIntent =
                    stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(pContext, PRIMARY_CHANNEL)
                    .SetContentTitle("Location chekin")
                    .SetSubText(pLocation.Date.ToLongTimeString())
                    .SetContentText("Long: " + pLocation.LongitudeValue + ", Lat: " + pLocation.LatitudeValue)
                    .SetSmallIcon(Resource.Drawable.logoApp2)
                    .SetAutoCancel(true);

            getNotificationManager(pContext).Notify(0, notificationBuilder.Build());
        }

        private NotificationManager getNotificationManager(Context pContext)
        {
            if (mNotificationManager == null)
            {
                mNotificationManager = (NotificationManager)pContext.GetSystemService(
                        Context.NotificationService);
            }
            return mNotificationManager;
        }
    }
}