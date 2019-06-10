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
using UserNotifications;
using Java.Util;

[assembly: Dependency(typeof(LocalNotifHelper_IOS))]
namespace SecurityTravelApp.iOS
{
    class LocalNotifHelper_IOS : ILocalNotifHelper
    {

        String categoryID;

        public LocalNotifHelper_IOS()
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            RegisterForNotifications();

        }


        /* implementation 1 */
        void RegisterForNotifications()
        {
            // Create action
            var actionID = "check";
            var title = "Check";
            var action = UNNotificationAction.FromIdentifier(actionID, title, UNNotificationActionOptions.Foreground);

            // Create category
            categoryID = "notification";
            var actions = new UNNotificationAction[] { action };
            var intentIDs = new string[] { };
            //var categoryOptions = new UNNotificationCategoryOptions[] { };
            var category = UNNotificationCategory.FromIdentifier(categoryID, actions, intentIDs, UNNotificationCategoryOptions.None);

            // Register category
            var categories = new UNNotificationCategory[] { category };
            UNUserNotificationCenter.Current.SetNotificationCategories(new NSSet<UNNotificationCategory>(categories));

            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert
                                                                  | UNAuthorizationOptions.Badge
                                                                  | UNAuthorizationOptions.Sound,
                                                                  (a, err) => {
                                                                      //TODO handle error
                                                                  });
        }

        void ScheduleNotification()
        {
            // local notification for IOS 10 and +
            //iOS 10 or above version
            var center = UNUserNotificationCenter.Current;
            var content = new UNMutableNotificationContent();
            content.Title = "Late wake up call";
            content.Body = "The early bird catches the worm, but the second mouse gets the cheese.";
            content.CategoryIdentifier = "alarm";
            content.UserInfo = new NSDictionary(new NSString("customData"), new NSString("fizzbuzz") );
            content.Sound = UNNotificationSound.Default;

            //var dateComponents = new DateComponents();
            //dateComponents.hour = 15;
            //dateComponents.minute = 49;
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(10, false);
            var request = UNNotificationRequest.FromIdentifier("198716876318", content, trigger);
            center.AddNotificationRequest(request, (err) => {
                if (err != null)
                {
                    // Report error
                    System.Console.WriteLine("Error: {0}", err);
                }
                else
                {
                    // Report Success
                    System.Console.WriteLine("Notification Scheduled: {0}", request);
                }
            });

            // Create content
            //var content = new UNMutableNotificationContent();
            //content.Title = "Title";
            //content.Subtitle = "Subtitle";
            //content.Body = "Body";
            //content.Badge = 1;
            //content.CategoryIdentifier = categoryID;
            //content.Sound = UNNotificationSound.Default;

            //// Fire trigger in one seconds
            //var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);

            //var requestID = "customNotification";
            //var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            ////                      This is the line that does the trick
            ////UNUserNotificationCenter.Current.Delegate = new CustomUNUserNotificationCenterDelegate();

            //UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => {
            //    if (err != null)
            //    {
            //        // Report error
            //        System.Console.WriteLine("Error: {0}", err);
            //    }
            //    else
            //    {
            //        // Report Success
            //        System.Console.WriteLine("Notification Scheduled: {0}", request);
            //    }
            //});
        }


        /* implementation 2 */

        





        /* first code */


        public void notifyTrackingOngoing()
        {
            ScheduleNotification();
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