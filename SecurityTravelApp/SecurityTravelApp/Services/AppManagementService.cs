using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SecurityTravelApp.Views;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SecurityTravelApp.Services
{
    class AppManagementService : ServiceAbstract
    {

        const ServiceType TYPE = ServiceType.AppManagement;

        private List<NavigationItem> navigationItems;


        private Dictionary<NavigationItemTarget, Page> ExistingPages;
        private Application AppReference;

        public Page CurrentPage;


        public AppManagementService() : base(TYPE)
        {
            ExistingPages = new Dictionary<NavigationItemTarget, Page>();
            navigationItems = new List<NavigationItem>();
            fillWithData();
        }

        public void config(Application pAppRef)
        {
            AppReference = pAppRef;
        }

        public Page lookUpPage(NavigationItemTarget pTarget)
        {
            if (ExistingPages.ContainsKey(pTarget))
            {
                return ExistingPages[pTarget];
            }
            else return null;

        }

        public Type TypeOfNavigationTarget(NavigationItemTarget pTarget)
        {
            switch (pTarget)
            {
                case NavigationItemTarget.Home: return typeof(HomePage);
                case NavigationItemTarget.Messages: return typeof(MessagesPage);
                case NavigationItemTarget.Warnings: return typeof(AlertsPage);
                case NavigationItemTarget.Login: return typeof(LoginPage);
                default: return typeof(PopupTestPage);
            }
        }


        private void navigateToAndSave(Page pPage, NavigationItemTarget pTarget)
        {
            if (!ExistingPages.ContainsKey(pTarget))
            {
                ExistingPages.Add(pTarget, pPage);
            }
            CurrentPage = pPage;
            AppReference.MainPage = CurrentPage;
        }

        public void incementNotif(NavigationItemTarget pType, int pIncrement = 1)
        {
            NavigationItem navItem = navigationItems.Find(x => x.target.Equals(pType));
            // if navItem is null ghir nmchi n3awd n9ra lbac
            navItem.carriesNotif = true;
            navItem.numberOfNotif += pIncrement;

            if (AppReference.MainPage is UpdatablePage)
            {
                UpdatablePage updatblePage = (UpdatablePage)AppReference.MainPage;
                updatblePage.update(new NavigationParams() { navigationUpdateTarget = pType });
            }

        }

        public void navigateTo(NavigationItemTarget pType, ServiceFactory pSrvFactory, NavigationParams pParamAfterNav)
        {
            Page targetPage = lookUpPage(pType);
            if (targetPage == null)
            {
                targetPage = (Page)Activator.CreateInstance(TypeOfNavigationTarget(pType), pSrvFactory, pParamAfterNav);
            }
            else
            {
                // pages should implement Updatable Page to allow UI updates
                UpdatablePage updatblePage = (UpdatablePage)targetPage;
                updatblePage.update(new NavigationParams() { navigationTarget = pType });
            }
            navigateToAndSave(targetPage, pType);
        }

        public List<NavigationItem> getTheNavigationItems()
        {
            return navigationItems;
        }

        public async Task<Boolean> checkForAllRequiredPermissions()
        {
            await checkForPermission(Permission.Phone);
            await checkForPermission(Permission.Location);
            return false;
        }

        public async Task<Boolean> checkForPermission(Permission pPermission)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(pPermission);
                if (status != PermissionStatus.Granted)
                {

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(pPermission);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(pPermission)) status = results[pPermission];

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

        private void fillWithData()
        {
            navigationItems.Add(createNavigationItem(2, "Home", NavigationItemTarget.Home, LineAwesomeIcons.Home, true, NavigationItemNotifType.Dot));
            navigationItems.Add(createNavigationItem(4, "Messages", NavigationItemTarget.Messages, LineAwesomeIcons.Messages, false));
            navigationItems.Add(createNavigationItem(3, "Warnings", NavigationItemTarget.Warnings, LineAwesomeIcons.Warnings, true, NavigationItemNotifType.Numerical, NavigationItemState.Current, 5));
            navigationItems.Add(createNavigationItem(5, "Docs", NavigationItemTarget.Docs, LineAwesomeIcons.Docs, true, NavigationItemNotifType.Numerical, NavigationItemState.Shaded, 2));
        }


        private NavigationItem createNavigationItem(int pId, String pText, NavigationItemTarget pTarget, String pSolidIcon, Boolean pCarriesNotif = true, NavigationItemNotifType pType = NavigationItemNotifType.Numerical, NavigationItemState pState = NavigationItemState.Shaded, int pNumberNotif = 0)
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
                notifType = pType,
                target = pTarget


            };
        }
    }
}
