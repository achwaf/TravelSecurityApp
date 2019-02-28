using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneCall_Droid))]
namespace SecurityTravelApp.Droid
{
    class PhoneCall_Droid : IPhoneCall
    {
        public async void makePhoneCall(String pNumber)
        {
            try
            {
                var uri = Android.Net.Uri.Parse(String.Format("tel:{0}", pNumber));
                var intent = new Intent(Intent.ActionCall, uri);
                intent.SetFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}