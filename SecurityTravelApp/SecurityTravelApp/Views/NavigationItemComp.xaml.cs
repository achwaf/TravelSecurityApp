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

        public String text { get; set; }
        public Boolean textVisible { get; set; }

        public NavigationItemComp(ServiceFactory pSrvFactory, NavigationItem pItem, Boolean HighlightFadeOut)
        {
            AppManagementService appMngService = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            InitializeComponent();
            BindingContext = pItem;

            // add tap gesture recognizer 
            var tapGestureRecognizerContainer = new TapGestureRecognizer();

            // setting the handler
            tapGestureRecognizerContainer.Tapped += async (s, e) =>
            {
                Page lastPage = appMngService.CurrentPage;
                Type typeAskedtPage = Utilities.TypeOfNavigationTarget(pItem.target);

                // effects on NavigationItem : highlighting
                showHighlight();
                if (lastPage.GetType() != typeAskedtPage)
                {
                    Page targetPage = appMngService.lookUpPage(pItem.target);
                    var paramAfterNav = new AfterNavigationParams() { navigationTarget = pItem.target };
                    if (targetPage == null)
                    {
                        targetPage = (Page)Activator.CreateInstance(typeAskedtPage, pSrvFactory, paramAfterNav);
                    }
                    else
                    {
                        // pages should implement Updatable Page to allow UI updates
                        UpdatablePage updatblePage = (UpdatablePage)targetPage;
                        updatblePage.update();
                    }
                    appMngService.navigateToAndSave(targetPage, pItem.target);
                    HighlightMask.IsVisible = false;
                }
                showAndFadeOutHighlight();

            };
            ItemContainer.GestureRecognizers.Add(tapGestureRecognizerContainer);

            if (HighlightFadeOut)
            {
                showAndFadeOutHighlight();
            }

            updateView(pItem);
        }

        private void updateView(NavigationItem pItem)
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

        private void showHighlight()
        {
            HighlightMask.Opacity = 0;
            HighlightMask.IsVisible = true;
            HighlightMask.FadeTo(.4, 100, Easing.CubicIn);

        }



        private async void showAndFadeOutHighlight()
        {
            HighlightMask.Opacity = .4;
            HighlightMask.IsVisible = true;
            await HighlightMask.FadeTo(0, 1000, Easing.CubicIn);
            HighlightMask.IsVisible = false;

        }

    }
}