using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Models;
using SecurityTravelApp.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationHelper_UWP))]
namespace SecurityTravelApp.UWP
{
    public class LocationHelper_UWP : LocationHelper
    {
        public event EventHandler LocationChanged;

        public void activateLocationUpdates(int IntervalTime)
        {
            throw new NotImplementedException();
        }

        public void disableLocationUpdates()
        {
            throw new NotImplementedException();
        }

        public Geoposition getLastKnownGPSLocation()
        {
            throw new NotImplementedException();
        }

        public Geoposition getLastKnownNetworkLocation()
        {
            throw new NotImplementedException();
        }
    }
}
