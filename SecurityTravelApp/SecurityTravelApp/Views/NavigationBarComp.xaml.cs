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
    public partial class NavigationBarComp : ContentView
    {
        private NavigationBarVM viewModel;
        private ServiceFactory srvFactory;


        public NavigationBarComp()
        {
            InitializeComponent();
        }

        public void initializeContent(ServiceFactory pSrvFactory, AfterNavigationParams pParams)
        {
            srvFactory = pSrvFactory;
            viewModel = new NavigationBarVM(pSrvFactory);
            BindingContext = viewModel;
            populateLayout(pParams);
        }

        private void populateLayout(AfterNavigationParams pParams)
        {
            // add NavigationItemComps to the layout
            var i = 0;
            foreach (var item in viewModel.navigationItems)
            {
                // create the component
                Boolean highlight = false;
                if (pParams != null)
                {
                    highlight = item.target == pParams.navigationTarget;
                }
                var comp = new NavigationItemComp(srvFactory, item, highlight);

                // add it
                Grid.SetColumn(comp, i++);
                HorizontalLayout.Children.Add(comp);
            }
        }

    }

}