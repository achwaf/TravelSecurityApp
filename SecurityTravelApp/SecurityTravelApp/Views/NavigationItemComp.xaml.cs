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
	public partial class NavigationItemComp : ContentView
	{

        public String text { get; set; }
        public Boolean textVisible { get; set; }

		public NavigationItemComp (NavigationItem pItem)
		{
			InitializeComponent ();
            BindingContext = pItem;
        }
	}
}