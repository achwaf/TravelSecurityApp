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
    public class LocationHelper_UWP : ILocationHelper
    {
        public event EventHandler LocationChanged;

        public void reactivateLocationUpdates(int IntervalTime)
        {
            // nothing
        }

        public void disableLocationUpdates()
        {
            // nothing
        }

        public Geoposition getLastKnownGPSLocation()
        {
            // nothing
            return null;
        }

        public Geoposition getLastKnownNetworkLocation()
        {
            // nothing
            return null;
        }
    }
}
