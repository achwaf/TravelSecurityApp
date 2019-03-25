using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.DependencyServices
{
    public interface LocationHelper
    {
        event EventHandler LocationChanged;
        Geoposition getLastKnownGPSLocation();
        Geoposition getLastKnownNetworkLocation();
        void disableLocationUpdates();
        void reactivateLocationUpdates(int IntervalTime);
    }
}
