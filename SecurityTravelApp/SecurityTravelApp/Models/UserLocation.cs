using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class UserLocation
    {
        public long latitude;
        public long longitude;
        public long altitude;
        public DateTime dateCheckin;

        public Boolean isSent;
        public DateTime dateSent;
    }
}
