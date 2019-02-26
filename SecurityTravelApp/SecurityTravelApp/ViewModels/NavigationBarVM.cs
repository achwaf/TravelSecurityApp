using SecurityTravelApp.Services;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SecurityTravelApp.ViewModels
{
    public class NavigationBarVM : BaseViewModel
    {

        AppManagementService appMngSrv;

        public ObservableCollection<NavigationItem> navigationItems { get; set; }

        public NavigationBarVM(ServiceFactory pSrvFactory)
        {
            navigationItems = new ObservableCollection<NavigationItem>();
            // call the service to retrieve data about navigation items
            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);

            //get the navigation items and fill the ViewModel
            fillWithNavigationItems(appMngSrv.getTheNavigationItems());
        }



        private void updateNavigationItems(List<NavigationItem> pItems)
        {
            // apply updates to the observableCollection so the UI changes accordingly 

            // compare the local navigation items with the retrieved list
            foreach (var item in pItems)
            {
                foreach (var observableItem in navigationItems)
                {
                    if (observableItem.id == item.id)
                    {
                        // update the underlying fields
                        if (observableItem.numberOfNotif != item.numberOfNotif) observableItem.numberOfNotif = item.numberOfNotif;
                        if (observableItem.state != item.state) observableItem.state = item.state;
                        if (observableItem.carriesNotif != item.carriesNotif) observableItem.carriesNotif = item.carriesNotif;
                        break;
                    }
                }
            }
        }

        private void fillWithNavigationItems(List<NavigationItem> pItems)
        {
            foreach (var item in pItems)
            {
                navigationItems.Add(item);
            }
        }
    }

}
