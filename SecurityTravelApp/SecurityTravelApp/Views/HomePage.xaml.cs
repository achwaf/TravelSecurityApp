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
    public partial class HomePage : ContentPage
    {
        public HomePage(ServiceFactory pSrvFactory, AfterNavigationParams pParam)
        {
            InitializeComponent();
            SosSlider.initializeConfig(pSrvFactory);
            NavigationBar.initializeContent(pSrvFactory, pParam);
        }
    }
}