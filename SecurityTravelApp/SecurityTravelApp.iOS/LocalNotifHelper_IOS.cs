using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.iOS;
using SecurityTravelApp.Models;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalNotifHelper_IOS))]
namespace SecurityTravelApp.iOS
{
    class LocalNotifHelper_IOS : ILocalNotifHelper
    {
        public void notifyTrackingOngoing()
        {
            showNotification("View Alert", "Your one minute alert has fired!");
        }

        public void notifyTrackingStopped()
        {
            showNotification("Travel Security", "Tracking stopped");
        }

        public void notifyTrackingUpdate(Geoposition pLocation)
        {
            showNotification("Update TS", "updated local notification");
        }

        private void showNotification(String pAction, String pBody)
        {
            // create the notification
            var notification = new UILocalNotification();

            // set the fire date (the date time in which it will fire)
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(0);

            // configure the alert
            notification.AlertAction = pAction;
            notification.AlertBody = pBody;

            // modify the badge
            notification.ApplicationIconBadgeNumber = 15;

            // set the sound to be the default sound
            notification.SoundName = UILocalNotification.DefaultSoundName;

            // schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }
    }
}