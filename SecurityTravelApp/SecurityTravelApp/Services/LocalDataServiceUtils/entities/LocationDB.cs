using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services.LocalDataServiceUtils.entities
{
    public class LocationDB
    {
        public float Accuracy { get; set; }
        public double Altitude { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public String Provider { get; set; }
        public DateTime DateCheckin { get; set; }
        public Boolean IsSOS { get; set; }

        public Boolean IsSent { get; set; }
        public DateTime DateSent { get; set; }
        public Boolean IsSendable { get; set; }
    }
}
