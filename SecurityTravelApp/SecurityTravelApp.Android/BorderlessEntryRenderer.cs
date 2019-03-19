﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SecurityTravelApp.Droid;
using SecurityTravelApp.Views.ViewsUtils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace SecurityTravelApp.Droid
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public static void Init() { }


        public BorderlessEntryRenderer(Context pContext) : base(pContext)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}