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
    public partial class NavigationBarComp : ContentView
    {
        private NavigationBarVM viewModel;
        private ServiceFactory srvFactory;

        public NavigationBarComp()
        {
            InitializeComponent();
        }

        public void initializeContent(ServiceFactory pSrvFactory)
        {
            srvFactory = pSrvFactory;
            viewModel = new NavigationBarVM(pSrvFactory);
            BindingContext = viewModel;
            populateLayout();
        }

        private void populateLayout()
        {
            // add NavigationItemComps to the layout
            foreach (var item in viewModel.navigationItems)
            {
                // create the component
                var comp = new NavigationItemComp(srvFactory,item);

                // add it
                HorizontalLayout.Children.Add(comp);

            }
        }
    }

}