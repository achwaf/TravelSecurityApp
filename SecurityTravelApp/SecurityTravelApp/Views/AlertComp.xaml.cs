using FFImageLoading.Forms;
using FFImageLoading.Svg.Forms;
using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Models;
using SecurityTravelApp.Utils;
using SecurityTravelApp.Views.Popups;
using SecurityTravelApp.Views.ViewsUtils;
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
    public partial class AlertComp : ContentView, I18nable
    {
        private Uri CriticSvg = new Uri("resource://SecurityTravelApp.Assets.CriticShadow.svg");
        private Uri WarningSvg = new Uri("resource://SecurityTravelApp.Assets.WarningShadow.svg");
        private Uri InfoSvg = new Uri("resource://SecurityTravelApp.Assets.InfoShadow.svg");

        private Color CriticColor = Color.FromHex("#D12A1A");
        private Color WarningColor = Color.FromHex("#E8BD4A");
        private Color InfoColor = Color.FromHex("#6CBEED");

        private Boolean ContinueFlashing = false;
        private Alert Item;
        Animation flashLoop;

        public AlertComp(Alert pAlert)
        {
            InitializeComponent();
            Item = pAlert;
            // create flashing animation
            flashLoop = new Animation();
            Animation fadeIn = new Animation(d => NewInfo.Opacity = d, .7, 1);
            Animation fadeOut = new Animation(d => NewInfo.Opacity = d, 1, .7);
            flashLoop.Add(0, .5, fadeIn);
            flashLoop.Add(.5, 1, fadeOut);

            // setting the taphandler   
            var tapGestureRecognizer = new TapGestureRecognizer();
            this.GestureRecognizers.Add(tapGestureRecognizer);

            tapGestureRecognizer.Tapped += async (s, e) =>
            {

                // update state of alert comp
                Item.isSeen = true;
                Item.dateSeen = DateTime.Now;
                dateRecievedAndSeen();

                Icon.FadeTo(.5, 50);
                await TextBackground.FadeTo(.85, 50);

                PopupNavigation.Instance.PushAsync(new AlertPopup(Item), false);

                Icon.FadeTo(1, 500);
                TextBackground.FadeTo(.65, 500);


                // send event to update in data base
                MessagingCenter.Send<AlertComp, Alert>(this, "ALERTSEEN", Item);
            };

            // set colors
            setColors();
            // date recieved and seen texts
            dateRecievedAndSeen();

            // filling texts
            Title.Text = pAlert.title;
            TheText.Text = pAlert.text;
            RegionText.Text = pAlert.region;

            // apply language
            updateTXT();
        }

        private void dateRecievedAndSeen()
        {
            // date recieved and seen region
            if (Item.isSeen)
            {
                ContinueFlashing = false;
                NewInfo.IsVisible = false;
                SeenInfo.IsVisible = true;

                // display time if seen today, display date elsewise
                if (Item.dateSeen.Date == DateTime.Today)
                {
                    TextSeen.Text = Item.dateSeen.ToShortTimeString();
                }
                else
                {
                    TextSeen.Text = Item.dateSeen.ToShortDateString();
                }
                Container.Opacity = .5;
            }
            else
            {
                Container.Opacity = 1;
                ContinueFlashing = true;
                NewInfo.IsVisible = true;
                SeenInfo.IsVisible = false;
                // luaunch the animation
                flashLoop.Commit(NewInfo, "flash", length: 200, repeat: () => { return ContinueFlashing; });
            }

            // display time if recieved today, display date elsewise
            if (Item.dateReceived.Date == DateTime.Today)
            {
                TextRecieved.Text = Item.dateReceived.ToShortTimeString();
            }
            else
            {
                TextRecieved.Text = Item.dateReceived.ToShortDateString();
            }
        }

        private void setColors()
        {
            // setting icon and colorIcon
            switch (Item.type)
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
        }

        public void updateTXT()
        {
            NewAlertTXT.Text = I18n.GetText(AppTextID.NEW);
        }
    }
}