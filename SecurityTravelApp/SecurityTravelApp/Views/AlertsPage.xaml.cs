using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
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
    public partial class AlertsPage : ContentPage, UpdatablePage, I18nable
    {
        LocalDataService localDataSrv;

        public AlertsPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            NavigationBar.initializeContent(pSrvFactory, pParam);

            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);

            populate();
        }

        private void populateAlerts(List<Alert> pAlerts)
        {
            foreach (var alert in pAlerts)
            {
                AlertComp alertComp = new AlertComp(alert);
                AlertsContainer.Children.Add(alertComp);
            }
            // adding spacer to be able to scroll up the last elements
            AlertsContainer.Children.Add(new BoxView() { HeightRequest = 60 });
        }

        private async void populate()
        {
            // clear 
            AlertsContainer.Children.Clear();

            // get data from server

            // get data locally
            List<Alert> listAlerts = await localDataSrv.getListAlert();
            populateAlerts(listAlerts);
        }

        public void updateTXT()
        {
            AlertTXT.Text = I18n.GetText(AppTextID.ALERTS);
            EmptyTXT.Text = I18n.GetText(AppTextID.EMPTY);

            

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