using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services
{
    class AppManagementService : ServiceAbstract
    {

        const ServiceType TYPE = ServiceType.AppManagement;

        private List<NavigationItem> navigationItems;

        public AppManagementService() : base(TYPE)
        {
            navigationItems = new List<NavigationItem>();
            fillWithTestData();
        }

        public List<NavigationItem> getTheNavigationItems()
        {
            return navigationItems;
        }

        private void fillWithTestData()
        {
            navigationItems.Add(createNavigationItemTest("More", FontAwesomeSolidIcons.PageMenuHorizontal));
            navigationItems.Add(createNavigationItemTest("Home", FontAwesomeSolidIcons.Branch));
            navigationItems.Add(createNavigationItemTest("Warnings", FontAwesomeSolidIcons.Device));
            navigationItems.Add(createNavigationItemTest("Messages", FontAwesomeSolidIcons.Check));
            navigationItems.Add(createNavigationItemTest("Intelligence", FontAwesomeSolidIcons.Crop));
        }


        private NavigationItem createNavigationItemTest(String pText, String pSolidIcon, Boolean pCarriesNotif = true, NavigationItemState pState = NavigationItemState.Active, int pNumberNotif = 99)
        {
            return new NavigationItem
            {
                text = pText,
                font = NavigationItemFont.FASolid,
                icon = pSolidIcon,
                carriesNotif = pCarriesNotif,
                state = pState,
                numberOfNotif = pNumberNotif
            };
        }
    }
}
