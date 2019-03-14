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

        static public void bounceAnimate(VisualElement pElement, double pElevation)
        {
            Animation mapMarkerAnimation;
            // create the bouncing animation
            mapMarkerAnimation = new Animation();
            var elevateAnimation = new Animation(d => pElement.TranslationY = d, 0, -pElevation, Easing.CubicOut);
            var bounceAnimation = new Animation(d => pElement.TranslationY = d, -pElevation, 0, Easing.BounceOut);
            mapMarkerAnimation.Add(0, .5, elevateAnimation);
            mapMarkerAnimation.Add(.5, 1, bounceAnimation);
            mapMarkerAnimation.Commit(pElement, "bounce", length: 1000);
        }

    }
}
