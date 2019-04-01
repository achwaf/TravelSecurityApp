using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTravelApp.Services
{
    public class ServerDataService : ServiceAbstract
    {

        const ServiceType TYPE = ServiceType.ServerData;

        public ServerDataService() : base(TYPE)
        {
        }

        public async Task<Boolean> sendDataToServer()
        {
            await Task.Delay(1000);
            Debug.WriteLine("data sent");
            return true;
        }
    }
}
