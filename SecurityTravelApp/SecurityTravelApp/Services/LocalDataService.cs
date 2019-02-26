using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services
{
    class LocalDataService : ServiceAbstract
    {
        const ServiceType TYPE = ServiceType.LocalData;

        public LocalDataService() : base(TYPE)
        {
        }
    }
}
