﻿using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using SecurityTravelApp.iOS;
using SecurityTravelApp.Views.ViewsUtils;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace SecurityTravelApp.iOS
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public static void Init() { }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(Control != null)
            {
                Control.Layer.BorderWidth = 0;
            }
        }
    }
}