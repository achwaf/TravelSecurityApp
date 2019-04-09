using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.DependencyServices
{
    public interface ILocalNotifHelper
    {
        void notifyTrackingOngoing();
        void notifyTrackingStopped();
        void notifyTrackingUpdate(Geoposition pLocation);
    }
}
