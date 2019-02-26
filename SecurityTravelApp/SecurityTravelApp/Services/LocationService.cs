using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services
{
    class LocationService : ServiceAbstract
    {

        const ServiceType TYPE = ServiceType.AppManagement;

        public LocationService() : base(TYPE)
        {
        }
    }
}
