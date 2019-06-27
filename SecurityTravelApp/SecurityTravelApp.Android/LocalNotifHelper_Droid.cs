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

        private Notification groupNotification;

        public LocalNotifHelper_Droid()
        {
            I18n.SelectLang(LocalDataService.getLanguagePreference());
            TrackingOngoing = I18n.GetText(AppTextID.TRACKING_ONGOING);
            TrackingStopped = I18n.GetText(AppTextID.TRACKING_STOPPED);
            groupNotification = constructGrouNotification().Build();
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



        void showNotificationUpdate(Geoposition pLocation)
        {
            Context context = Android.App.Application.Context;
            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(context, PRIMARY_CHANNEL)
                    .SetContentTitle(I18n.GetText(AppTextID.LAST_CHECKIN))
                    .SetSmallIcon(Resource.Drawable.AppNotifIcon)
                    .SetSubText(pLocation.Date.ToLongTimeString())
                    .SetContentText("Long: " + pLocation.LongitudeValue + ", Lat: " + pLocation.LatitudeValue)
                    .SetOngoing(false)
                    .SetShowWhen(false)
                    .SetGroup(GROUP_KEY_TRACKING)
                    .SetGroupSummary(true);

            getNotificationManager(context).Notify(NOTIF_ID_2, notificationBuilder.Build());
        }


        NotificationCompat.Builder constructGrouNotification()
        {
            Context context = Android.App.Application.Context;
            String content = "Group Content Text";
            NotificationCompat.Builder groupBuilder =
            new NotificationCompat.Builder(context)
                    .SetContentTitle("Group Notification")
                    .SetSmallIcon(Resource.Drawable.AppNotifIcon)
                    .SetContentText(content)
                    .SetGroupSummary(true)
                    .SetGroup(GROUP_KEY_TRACKING)
                    .SetStyle(new NotificationCompat.BigTextStyle().BigText(content));

            return groupBuilder;
        }


        void showNotificationTrackingInfo(Boolean pIsUserStillTracked)
        {
            Context context = Android.App.Application.Context;

            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(context, PRIMARY_CHANNEL)
                    .SetContentTitle(pIsUserStillTracked ? TrackingOngoing : TrackingStopped)
                    .SetSmallIcon(Resource.Drawable.AppNotifIcon)
                    .SetOngoing(pIsUserStillTracked)
                    .SetGroup(GROUP_KEY_TRACKING)
                    .SetShowWhen(false)
                    .SetAutoCancel(false);

            getNotificationManager(context).Notify(NOTIF_ID_0, groupNotification);
            getNotificationManager(context).Notify(NOTIF_ID_1, notificationBuilder.Build());
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