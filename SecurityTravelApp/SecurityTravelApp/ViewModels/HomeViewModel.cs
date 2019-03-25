using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        private Geoposition _geoposition;
        public Geoposition geoposition
        {
            get
            {
                return _geoposition;
            }

            set
            {
                SetProperty(ref _geoposition, value);
            }
        }
    }
}
