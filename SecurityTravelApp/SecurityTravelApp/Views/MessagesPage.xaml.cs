using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Services;
using SecurityTravelApp.ViewModels;
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
    public partial class MessagesPage : ContentPage
    {
        IKeyboardCheck keyboardService;

        public MessagesPage(ServiceFactory pSrvFactory, AfterNavigationParams pParam)
        {
            InitializeComponent();
            NavigationBar.initializeContent(pSrvFactory, pParam);


        }

        private void KeyboardService_KeyboardIsHidden(object sender, EventArgs e)
        {
            ThePage.Padding = new Thickness(0);
        }

        private void KeyboardService_KeyboardIsShown(object sender, EventArgs e)
        {
            ThePage.Padding = new Thickness(0, 0, 0, 200);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //keyboardService.KeyboardIsShown += KeyboardService_KeyboardIsShown;
            //keyboardService.KeyboardIsHidden += KeyboardService_KeyboardIsHidden;

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //keyboardService.KeyboardIsShown -= KeyboardService_KeyboardIsShown;
            //keyboardService.KeyboardIsHidden -= KeyboardService_KeyboardIsHidden;
        }
    }
}