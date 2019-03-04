using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using SecurityTravelApp.Droid;
using SecurityTravelApp.DependencyServices;
using Android.Views.InputMethods;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeyboardCheck_Droid))]
namespace SecurityTravelApp.Droid
{
    public class KeyboardCheck_Droid : Java.Lang.Object, IKeyboardCheck, ViewTreeObserver.IOnGlobalLayoutListener
    {

        public event EventHandler KeyboardIsShown;
        public event EventHandler KeyboardIsHidden;

        private InputMethodManager inputMethodManager;

        private bool wasShown = false;

        public KeyboardCheck_Droid()
        {
            GetInputMethodManager();
            SubscribeEvents();
        }

        public void OnGlobalLayout()
        {
            GetInputMethodManager();
            if (!wasShown && IsCurrentlyShown())
            {
                KeyboardIsShown?.Invoke(this, EventArgs.Empty);
                wasShown = true;
            }
            else if (wasShown && !IsCurrentlyShown())
            {
                KeyboardIsHidden?.Invoke(this, EventArgs.Empty);
                wasShown = false;
            }
        }

        private Boolean IsCurrentlyShown()
        {
            return inputMethodManager.IsAcceptingText;
        }

        private void GetInputMethodManager()
        {
            if (inputMethodManager == null || inputMethodManager.Handle == IntPtr.Zero)
            {
                inputMethodManager = (InputMethodManager)MainActivity.TheInstance.GetSystemService(Context.InputMethodService);
            }
        }

        private void SubscribeEvents()
        {
            ((Activity)MainActivity.TheInstance).Window.DecorView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }
        
    }
}

