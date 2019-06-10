using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneCall_IOS))]
namespace SecurityTravelApp.iOS
{
    class PhoneCall_IOS : IPhoneCall
    {
        public void makePhoneCall(string pNumber)
        {
            try
            {
                var url = new Foundation.NSUrl("tel:" + pNumber);
                UIApplication.SharedApplication.OpenUrl(url);
            }
            catch (Exception e)
            {
                String msg = e.Message;
            }
        }
    }
}