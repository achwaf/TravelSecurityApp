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
    public partial class NavigationItemComp : ContentView
    {


        public NavigationItem Item;

        public NavigationItemComp(ServiceFactory pSrvFactory, NavigationItem pItem, Boolean HighlightFadeOut)
        {
            AppManagementService appMngService = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            InitializeComponent();
            Item = pItem;
            BindingContext = Item;

            // add tap gesture recognizer 
            var tapGestureRecognizerContainer = new TapGestureRecognizer();

            // setting the handler
            tapGestureRecognizerContainer.Tapped += async (s, e) =>
            {
                Page lastPage = appMngService.CurrentPage;
                Type typeAskedtPage = appMngService.TypeOfNavigationTarget(pItem.target);

                // effects on NavigationItem : highlighting
                showHighlight();

                // remove the notification 
                pItem.carriesNotif = false;
                pItem.numberOfNotif = 0;

                // action
                if (lastPage.GetType() != typeAskedtPage)
                {
                    var paramAfterNav = new NavigationParams() { navigationTarget = pItem.target };
                    appMngService.navigateTo(pItem.target, pSrvFactory, paramAfterNav);
                    HighlightMask.IsVisible = false;
                }
                showAndFadeOutHighlight();

            };
            ItemContainer.GestureRecognizers.Add(tapGestureRecognizerContainer);

            if (HighlightFadeOut)
            {
                showAndFadeOutHighlight();
            }

            SetView(pItem);
        }

        private void SetView(NavigationItem pItem)
        {
            // notification type
            if (pItem.notifType == NavigationItemNotifType.Numerical)
            {
                NumercialNotif.IsVisible = pItem.carriesNotif;
                DotNotif.IsVisible = false;
            }
            else if (pItem.notifType == NavigationItemNotifType.Dot)
            {
                NumercialNotif.IsVisible = false;
                DotNotif.IsVisible = pItem.carriesNotif;
            }

        }

        public void showHighlight()
        {
            HighlightMask.Opacity = 0;
            HighlightMask.IsVisible = true;
            HighlightMask.FadeTo(.4, 100, Easing.CubicIn);

        }



        public async void showAndFadeOutHighlight()
        {
            HighlightMask.Opacity = .4;
            HighlightMask.IsVisible = true;
            await HighlightMask.FadeTo(0, 1000, Easing.CubicIn);
            HighlightMask.IsVisible = false;

        }

    }
}