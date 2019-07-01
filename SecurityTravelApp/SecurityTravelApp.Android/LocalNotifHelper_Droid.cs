using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Droid;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalNotifHelper_Droid))]
namespace SecurityTravelApp.Droid
{
    class LocalNotifHelper_Droid : ILocalNotifHelper
    {

        private String TrackingOngoing;
        private String TrackingStopped;
        private NotificationManager mNotificationManager;
        private static String PRIMARY_CHANNEL = "default";
        private String GROUP_KEY_TRACKING = "Travel.Security.Tracking";
        private int NOTIF_ID_0 = 0;
        private int NOTIF_ID_1 = 1;
        private int NOTIF_ID_2 = 2;
        private int NOTIF_ID_3 = 3;
        

        public LocalNotifHelper_Droid()
        {
            I18n.SelectLang(LocalDataService.getLanguagePreference());
            TrackingOngoing = I18n.GetText(AppTextID.TRACKING_ONGOING);
            TrackingStopped = I18n.GetText(AppTextID.TRACKING_STOPPED);
        }

        public void notifyTrackingOngoing()
        {
            //showNotificationTrackingInfo(true);
            showMultiLineNotificationTracking(true, null);
        }

        public void notifyTrackingStopped()
        {
            //showNotificationTrackingInfo(false);
            showMultiLineNotificationTracking(false, null);
        }

        public void notifyTrackingUpdate(Geoposition pLocation)
        {
            //showNotificationUpdate(pLocation);
            showMultiLineNotificationTracking(true, pLocation);

        }

        void showMultiLineNotificationTracking(Boolean pIsUserStillTracked, Geoposition pLocation)
        {
            
            String CHANNEL_ID = "TS_01";// The id of the channel. 
            String title = pIsUserStillTracked ? TrackingOngoing : TrackingStopped;
            Context context = Android.App.Application.Context;
            NotificationChannel mChannel = new NotificationChannel(CHANNEL_ID, "Travel Security" , NotificationImportance.High);
            NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(context)
                    .SetSmallIcon(Resource.Drawable.AppNotifIcon)
                    .SetContentTitle("Travel Security")
                    .SetContentText(title)
                    .SetChannelId(CHANNEL_ID);

            if (pLocation != null)
            {
                // when the user pulls the notification
                NotificationCompat.InboxStyle inboxStyle = new NotificationCompat.InboxStyle();
                inboxStyle.SetBigContentTitle(title);
                inboxStyle.AddLine(I18n.GetText(AppTextID.LAST_CHECKIN));
                inboxStyle.AddLine("Long: " + pLocation.LongitudeValue + ", Lat: " + pLocation.LatitudeValue);
                inboxStyle.AddLine(I18n.GetText(AppTextID.AT) + " " + pLocation.Date.ToLongTimeString());
                mBuilder.SetStyle(inboxStyle);
            }

            mBuilder.SetCategory(Notification.CategoryError);
            mBuilder.SetOngoing(pIsUserStillTracked);
            mBuilder.SetShowWhen(false);
            mBuilder.SetAutoCancel(false);
            //getNotificationManager(context).CancelAll();
            getNotificationManager(context).CreateNotificationChannel(mChannel);
            getNotificationManager(context).Notify(NOTIF_ID_3, mBuilder.Build());
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