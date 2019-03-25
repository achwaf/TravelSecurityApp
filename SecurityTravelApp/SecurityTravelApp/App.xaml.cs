using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SecurityTravelApp.Views;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
using SecurityTravelApp.Views.ViewsUtils;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SecurityTravelApp
{
    public partial class App : Application
    {
        private ServiceFactory serviceFactory;
        private AppManagementService appMngSrv;

        public App()
        {
            InitializeComponent();
            serviceFactory = new ServiceFactory();

            // instantiating services
            appMngSrv = (AppManagementService)serviceFactory.getService(ServiceType.AppManagement);

            // setting services dependencies and configs
            appMngSrv.config(this);

            // Selecting App language
            I18n.SelectLang(AppLanguage.EN);

            appMngSrv.navigateTo(NavigationItemTarget.Login, serviceFactory, null);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
