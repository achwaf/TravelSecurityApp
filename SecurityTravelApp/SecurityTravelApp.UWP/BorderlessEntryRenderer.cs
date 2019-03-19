using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using SecurityTravelApp.UWP;
using SecurityTravelApp.Views.ViewsUtils;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace SecurityTravelApp.UWP
{
    class BorderlessEntryRenderer : EntryRenderer
    {
        public static void Init() { }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
                Control.Margin = new Windows.UI.Xaml.Thickness(0);
                Control.Padding = new Windows.UI.Xaml.Thickness(0);
            }
        }
    }
}





