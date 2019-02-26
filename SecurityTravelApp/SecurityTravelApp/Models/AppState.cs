using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class AppState
    {
        public Boolean gpsEnabled;
        public Boolean localNetworkEnabled;
        public Boolean internetDataEnabled;

        public DateTime dateLastStorageCleanning;
        public DateTime dateLastAppParamUpdate;

    }
}
