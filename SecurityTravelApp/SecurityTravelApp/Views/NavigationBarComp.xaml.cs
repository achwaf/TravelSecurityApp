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

        public NavigationBarComp()
        {
            InitializeComponent();
        }

        public void initializeContent(NavigationBarVM pViewModel)
        {
            viewModel = pViewModel;
            BindingContext = viewModel;
            populateLayout();
        }

        private void populateLayout()
        {
            // add NavigationItemComps to the layout
            foreach (var item in viewModel.navigationItems)
            {
                // create the component
                var comp = new NavigationItemComp(item);

                // add it
                HorizontalLayout.Children.Add(comp);

            }
        }
    }

}