using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SecurityTravelApp.Services
{
    public class LocalNotifService : ServiceAbstract
    {

        const ServiceType TYPE = ServiceType.LocalNotif;

        ILocalNotifHelper localNotifHelper;

        public LocalNotifService() : base(TYPE)
        {
            localNotifHelper = DependencyService.Get<ILocalNotifHelper>();
        }

        public void showNotifTrackingOn()
        {
            localNotifHelper.notifyTrackingOngoing();
        }

        public void showNotifTrackingOff()
        {
            localNotifHelper.notifyTrackingStopped();
        }


        public void showNotifTrackingUpdate(Geoposition pLocation)
        {
            localNotifHelper.notifyTrackingUpdate(pLocation);
        }

    }
}
