using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
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
    public partial class AlertsPage : ContentPage, UpdatablePage
    {
        LocalDataService localDataSrv;

        public AlertsPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            NavigationBar.initializeContent(pSrvFactory, pParam);

            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);
            populateAlerts(localDataSrv.getAlerts());

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



        public void update(NavigationParams pParam)
        {
            // update navigation bar
            NavigationBar.update(pParam);
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