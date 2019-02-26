using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services
{
    class ServerDataService : ServiceAbstract
    {

        const ServiceType TYPE = ServiceType.ServerData;

        public ServerDataService() : base(TYPE)
        {
        }
    }
}
