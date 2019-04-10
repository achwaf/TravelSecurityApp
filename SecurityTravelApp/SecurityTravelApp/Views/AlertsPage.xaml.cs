using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Services.LocalDataServiceUtils.entities;
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
    public partial class AlertsPage : ContentPage, Updatable, I18nable
    {
        LocalDataService localDataSrv;
        private Boolean IsCriticFilterSet = true;
        private Boolean IsWarningFilterSet = true;
        private Boolean IsInfoFilterSet = true;
        private List<Alert> listAlerts;

        private Color FilterOnColor = Color.FromHex("#D12A1A");
        private Color FilterOffColor = Color.FromHex("#C8C8C8");

        public AlertsPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            NavigationBar.initializeContent(pSrvFactory, pParam);

            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);

            // subscribe to alert seen events 
            MessagingCenter.Subscribe<AlertComp, Alert>(this, "ALERTSEEN", async (sender, alert) =>
                {
                    AlertDB alertDB = await localDataSrv.getAlertDB(alert);
                    alertDB.DateSeen = alert.dateSeen;
                    alertDB.IsSeen = alert.isSeen;
                    localDataSrv.updateToDB(alertDB);
                });

            // subscribe to alert filter events 
            MessagingCenter.Subscribe<FilterAlertPopup, FilterAlertMessage>(this, "ALERTFILTER", (sender, message) =>
                {
                    if (message.Filter == AlertType.Critical)
                    {
                        IsCriticFilterSet = message.IsSet;
                    }
                    else if (message.Filter == AlertType.Important)
                    {
                        IsWarningFilterSet = message.IsSet;
                    }
                    else if (message.Filter == AlertType.Normal)
                    {
                        IsInfoFilterSet = message.IsSet;
                    }
                    filterAlerts();
                });



            // tap gesture recognizers
            var tapGestureRecognizerFilterIcon = new TapGestureRecognizer();
            FilterIcon.GestureRecognizers.Add(tapGestureRecognizerFilterIcon);

            // setting the handler to FilterIcon
            tapGestureRecognizerFilterIcon.Tapped += (s, e) =>
            {
                PopupNavigation.Instance.PushAsync(new FilterAlertPopup(IsCriticFilterSet, IsWarningFilterSet, IsInfoFilterSet), false);
            };

            populate();
            updateTXT();
        }

        private void populateAlerts(List<Alert> pAlerts)
        {
            if (pAlerts.Count == 0)
            {
                EmptyInfo.IsVisible = true;
            }
            else
            {
                foreach (var alert in pAlerts)
                {
                    AlertComp alertComp = new AlertComp(alert);
                    AlertsContainer.Children.Add(alertComp);
                }
                // adding spacer to be able to scroll up the last elements
                AlertsContainer.Children.Add(new BoxView() { HeightRequest = 60 });
            }
        }

        private void filterAlerts()
        {
            if(IsCriticFilterSet && IsWarningFilterSet && IsInfoFilterSet)
            {
                FilterIcon.TextColor = FilterOffColor;
            }
            else
            {
                FilterIcon.TextColor = FilterOnColor;
            }
            int notVisibleCounter = 0;
            for (int i = 0; i < listAlerts.Count; i++)
            {
                
                var alert = listAlerts[i];
                if (alert.type == AlertType.Critical && IsCriticFilterSet)
                {
                    AlertsContainer.Children[i].IsVisible = true;
                }
                else if (alert.type == AlertType.Important && IsWarningFilterSet)
                {
                    AlertsContainer.Children[i].IsVisible = true;
                }
                else if (alert.type == AlertType.Normal && IsInfoFilterSet)
                {
                    AlertsContainer.Children[i].IsVisible = true;
                }
                else
                {
                    AlertsContainer.Children[i].IsVisible = false;
                    notVisibleCounter++;
                }
            }
            if (notVisibleCounter < listAlerts.Count)
            {
                EmptyFilterInfo.IsVisible = false;
            }
            else
            {
                EmptyFilterInfo.IsVisible = true;
            }
        }

        private async void populate()
        {
            // clear 
            AlertsContainer.Children.Clear();

            // get data from server

            // get data locally
            listAlerts = await localDataSrv.getListAlert();
            populateAlerts(listAlerts);
            filterAlerts();
        }

        public void updateTXT()
        {
            AlertTXT.Text = I18n.GetText(AppTextID.ALERTS);
            EmptyTXT.Text = I18n.GetText(AppTextID.EMPTY);
            EmptyTXT2.Text = I18n.GetText(AppTextID.EMPTY);
            FilterEmptyTXT.Text = I18n.GetText(AppTextID.FILTER_EMPTY);
            NavigationBar.updateTXT();
        }

        public void update(NavigationParams pParam)
        {
            // update navigation bar
            NavigationBar.update(pParam);
            // update data
            if (!pParam.NavigationBarOnly)
            {
                populate();
            }
        }

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            //if (PopupNavigation.Instance.PopupStack.Count > 0)
            //{
            //    PopupNavigation.Instance.PopAsync();
            //}
            return true;
        }
    }
}