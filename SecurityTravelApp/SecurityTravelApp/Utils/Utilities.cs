using SecurityTravelApp.Views;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SecurityTravelApp.Utils
{
    class Utilities
    {

        public static Type TypeOfNavigationTarget(NavigationItemTarget pTarget)
        {
            switch (pTarget)
            {
                case NavigationItemTarget.Home: return typeof(HomePage);
                case NavigationItemTarget.Messages: return typeof(MessagesPage);
                default: return typeof(LoginPage);
            }
        }

        static public T getOnPlatformValue<T>(Object pSource)
        {
            T value = default(T);
            var onPlatform = (OnPlatform<T>)pSource;
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                value = (T)onPlatform.Android;
            }
            foreach (var on in onPlatform.Platforms)
            {
                if (on.Platform[0].Equals(Device.RuntimePlatform))
                {
                    value = (T)on.Value;
                    break;
                }
            }
            if (value == null || value.Equals(default(T)))
            {
                value = (T)onPlatform.Default;
            }
            return value;
        }
    }
}
