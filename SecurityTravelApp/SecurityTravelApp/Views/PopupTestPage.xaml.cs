using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Services;
using SecurityTravelApp.Views.Popups;
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
    public partial class PopupTestPage : ContentPage
    {

        AppManagementService appMngSrv;
        LocalDataService localDataSrv;

        public PopupTestPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await localDataSrv.getListLocation();
            await localDataSrv.getLastPosition();
        }
    }
}