using FFImageLoading.Forms;
using FFImageLoading.Svg.Forms;
using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlertComp : ContentView
    {
        private Uri CriticSvg = new Uri("resource://SecurityTravelApp.Assets.CriticShadow.svg");
        private Uri WarningSvg = new Uri("resource://SecurityTravelApp.Assets.WarningShadow.svg");
        private Uri InfoSvg = new Uri("resource://SecurityTravelApp.Assets.InfoShadow.svg");

        private Color CriticColor = Color.FromHex("#D12A1A");
        private Color WarningColor = Color.FromHex("#E8BD4A");
        private Color InfoColor = Color.FromHex("#6CBEED");

        private Boolean ContinueFlashing = false;


        public AlertComp(Alert pAlert)
        {
            InitializeComponent();



            // create flashing animation
            Animation flashLoop = new Animation();
            Animation fadeIn = new Animation(d => NewInfo.Opacity = d, .7, 1);
            Animation fadeOut = new Animation(d => NewInfo.Opacity = d, 1, .7);
            flashLoop.Add(0, .5, fadeIn);
            flashLoop.Add(.5, 1, fadeOut);

            // setting the taphandler   
            var tapGestureRecognizer = new TapGestureRecognizer();
            this.GestureRecognizers.Add(tapGestureRecognizer);

            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                Icon.FadeTo(.5, 50);
                await TextBackground.FadeTo(.85, 50);

                Icon.FadeTo(1, 500);
                TextBackground.FadeTo(.65, 500);
            };


            // setting icon and colorIcon
            switch (pAlert.type)
            {
                case AlertType.Critical:
                    AlertIcon.Source = new EmbeddedResourceImageSource(CriticSvg);
                    IconBackground.Color = CriticColor;
                    break;
                case AlertType.Important:
                    AlertIcon.Source = new EmbeddedResourceImageSource(WarningSvg);
                    IconBackground.Color = WarningColor;
                    break;
                case AlertType.Normal:
                    AlertIcon.Source = new EmbeddedResourceImageSource(InfoSvg);
                    IconBackground.Color = InfoColor;
                    break;
            }

            // filling texts
            Title.Text = pAlert.title;
            TheText.Text = pAlert.text;
            RegionText.Text = pAlert.region;


            // date recieved and seen region
            if (pAlert.isSeen)
            {
                NewInfo.IsVisible = false;
                SeenInfo.IsVisible = true;

                // display time if seen today, display date elsewise
                if (pAlert.dateSeen.Date == DateTime.Today)
                {
                    TextSeen.Text = pAlert.dateSeen.ToShortTimeString();
                }
                else
                {
                    TextSeen.Text = pAlert.dateSeen.ToShortDateString();
                }
            }
            else
            {
                ContinueFlashing = true;
                NewInfo.IsVisible = true;
                SeenInfo.IsVisible = false;
                // luaunch the animation
                flashLoop.Commit(NewInfo, "flash", length: 200, repeat: () => { return true; });
            }

            // display time if recieved today, display date elsewise
            if (pAlert.dateReceived.Date == DateTime.Today)
            {
                TextRecieved.Text = pAlert.dateReceived.ToShortTimeString();
            }
            else
            {
                TextRecieved.Text = pAlert.dateReceived.ToShortDateString();
            }

        }
    }
}