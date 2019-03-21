using FFImageLoading.Forms;
using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlertPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private Uri CriticSvg = new Uri("resource://SecurityTravelApp.Assets.CriticShadow.svg");
        private Uri WarningSvg = new Uri("resource://SecurityTravelApp.Assets.WarningShadow.svg");
        private Uri InfoSvg = new Uri("resource://SecurityTravelApp.Assets.InfoShadow.svg");

        private Color CriticColor = Color.FromHex("#D12A1A");
        private Color WarningColor = Color.FromHex("#E8BD4A");
        private Color InfoColor = Color.FromHex("#6CBEED");

        public AlertPopup(Alert pAlert)
        {
            InitializeComponent();

            // correct the behavior of backgroud tapping in android
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                var tapGestureRecognizerContainer = new TapGestureRecognizer();
                tapGestureRecognizerContainer.Tapped += (s, e) =>
                {
                    PopupNavigation.Instance.RemovePageAsync(this);
                };
                container.GestureRecognizers.Add(tapGestureRecognizerContainer);
            }

            // setting icon and colorIcon
            switch (pAlert.type)
            {
                case AlertType.Critical:
                    AlertIcon.Source = new EmbeddedResourceImageSource(CriticSvg);
                    FrameBackground.BackgroundColor = CriticColor;
                    break;
                case AlertType.Important:
                    AlertIcon.Source = new EmbeddedResourceImageSource(WarningSvg);
                    FrameBackground.BackgroundColor = WarningColor;
                    break;
                case AlertType.Normal:
                    AlertIcon.Source = new EmbeddedResourceImageSource(InfoSvg);
                    FrameBackground.BackgroundColor = InfoColor;
                    break;
            }

            // filling texts
            Title.Text = pAlert.title;
            TheText.Text = pAlert.text;
            RegionText.Text = pAlert.region;


            // date recieved and seen region
            TextSeen.Text = pAlert.dateSeen.ToString();
            TextRecieved.Text = pAlert.dateReceived.ToString();
        }
    }
}