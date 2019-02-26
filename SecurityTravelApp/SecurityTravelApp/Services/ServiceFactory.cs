using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services
{
    public class ServiceFactory
    {
        private static List<ServiceAbstract> ListRunningServices = new List<ServiceAbstract>();


        public ServiceFactory()
        {
        }


        public ServiceAbstract getService(ServiceType pService)
        {
            ServiceAbstract vService = null;
            // browse the static list to get the service
            // if not found instanciate it
            foreach (var service in ListRunningServices)
            {
                if (service.Type.Equals(pService))
                {
                    vService = service;
                    break;
                }
            }
            if (vService == null)
            {
                vService = createService(pService);
                ListRunningServices.Add(vService);
            }

            return vService;
        }

        private ServiceAbstract createService(ServiceType pService)
        {
            ServiceAbstract vService = null;
            switch (pService)
            {
                case ServiceType.Empty:
                    vService = null;
                    break;
                case ServiceType.Location:
                    vService = new LocationService();
                    break;
                case ServiceType.LocalData:
                    vService = new LocalDataService();
                    break;
                case ServiceType.ServerData:
                    vService = new ServerDataService();
                    break;
                case ServiceType.AppManagement:
                    vService = new AppManagementService();
                    break;
            }

            return vService;
        }


    }

    public enum ServiceType
    {
        Empty,
        Location,
        LocalData,
        ServerData,
        AppManagement
    }
}
