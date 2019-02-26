using SecurityTravelApp.Services;
using SecurityTravelApp.ViewModels;
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
        public HomePage(ServiceFactory pSrvFactory)
        {
            InitializeComponent();
            NavigationBar.initializeContent(new NavigationBarVM(pSrvFactory));
        }
    }
}