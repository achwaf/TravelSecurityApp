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


        public AlertComp(Alert pAlert)
        {
            InitializeComponent();


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
        }
    }
}