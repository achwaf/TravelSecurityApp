using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class UserLocation
    {
        public Geoposition geoposition;
        public DateTime dateCheckin;

        public Boolean isSent;
        public DateTime dateSent;
    }
}
