using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Boolean> checkForPhonePermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Phone);
                if (status != PermissionStatus.Granted)
                {

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Phone);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Phone)) status = results[Permission.Phone];

                    if (status == PermissionStatus.Granted)
                    {
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void fillWithTestData()
        {
            navigationItems.Add(createNavigationItemTest(1, "More", FontAwesomeSolidIcons.PageMenuVertical, false));
            navigationItems.Add(createNavigationItemTest(2, "Home", FontAwesomeSolidIcons.Branch, true, NavigationItemNotifType.Dot));
            navigationItems.Add(createNavigationItemTest(3, "Warnings", FontAwesomeSolidIcons.Device,true,NavigationItemNotifType.Numerical,NavigationItemState.Current,5));
            navigationItems.Add(createNavigationItemTest(4, "Messages", FontAwesomeSolidIcons.Check, false));
            navigationItems.Add(createNavigationItemTest(5, "Intelligence", FontAwesomeSolidIcons.Crop, true, NavigationItemNotifType.Numerical,NavigationItemState.Shaded,2));
        }


        private NavigationItem createNavigationItemTest(int pId, String pText, String pSolidIcon, Boolean pCarriesNotif = true, NavigationItemNotifType pType = NavigationItemNotifType.Numerical, NavigationItemState pState = NavigationItemState.Shaded, int pNumberNotif = 99)
        {
            return new NavigationItem
            {
                id = pId,
                text = pText,
                font = NavigationItemFont.FASolid,
                icon = pSolidIcon,
                carriesNotif = pCarriesNotif,
                state = pState,
                numberOfNotif = pNumberNotif,
                notifType = pType

            };
        }
    }
}
