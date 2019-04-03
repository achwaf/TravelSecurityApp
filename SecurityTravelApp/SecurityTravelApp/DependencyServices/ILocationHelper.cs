using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.DependencyServices
{
    public interface ILocationHelper
    {
        event EventHandler LocationChanged;
        Geoposition getLastKnownGPSLocation();
        Geoposition getLastKnownNetworkLocation();
        void disableLocationUpdates();
        void disableBackgroundLocationUpdates();
        void reactivateLocationUpdates(int IntervalTime);
        void activateBackgroundLocationUpdates(int IntervalTime);
    }
}
