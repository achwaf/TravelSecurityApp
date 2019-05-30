using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManualTrackingPopup : Rg.Plugins.Popup.Pages.PopupPage, I18nable
    {
        public ManualTrackingPopup()
        {
            InitializeComponent();
        }

        public void updateTXT()
        {
            throw new NotImplementedException();
        }
    }
}