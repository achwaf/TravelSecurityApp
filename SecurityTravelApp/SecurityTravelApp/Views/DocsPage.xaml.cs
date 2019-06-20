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
	public partial class DocsPage : ContentPage , Updatable, I18nable
    {
        LocalDataService localDataSrv;

        public DocsPage (ServiceFactory pSrvFactory, NavigationParams pParam)
		{
			InitializeComponent ();

            NavigationBar.initializeContent(pSrvFactory, pParam);
            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);
        }

        public void update(NavigationParams pParam)
        {
            
        }

        public void updateTXT()
        {
            
        }
    }
}