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

        public NavigationItemComp(ServiceFactory pSrvFactory, NavigationItem pItem)
        {
            AppManagementService appService = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            InitializeComponent();
            BindingContext = pItem;

            // add tap gesture recognizer 
            var tapGestureRecognizerContainer = new TapGestureRecognizer();
            tapGestureRecognizerContainer.Tapped += async (s, e) =>
            {
                Page page = (Page)Activator.CreateInstance(Utilities.TypeOfNavigationTarget(pItem.target), pSrvFactory);
                await Navigation.PushAsync(page, false);
                Navigation.RemovePage(Navigation.NavigationStack.ElementAt(Navigation.NavigationStack.Count - 2));

            };
            ItemContainer.GestureRecognizers.Add(tapGestureRecognizerContainer);

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
    }
}