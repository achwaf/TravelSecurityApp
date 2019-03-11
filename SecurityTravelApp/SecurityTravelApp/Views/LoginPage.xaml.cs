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
    public partial class LoginPage : ContentPage, UpdatablePage
    {
        public LoginPage(ServiceFactory pSrvFactory, AfterNavigationParams pParam)
        {
            InitializeComponent();
            NavigationBar.initializeContent(pSrvFactory, pParam);
        }

        public void update()
        {
        }
    }
}