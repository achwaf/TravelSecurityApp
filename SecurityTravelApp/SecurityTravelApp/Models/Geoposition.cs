using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class Geoposition
    {

        public bool HasAccuracy { get; set; }
        public float Accuracy { get; set; }
        public double Altitude { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public String Provider { get; set; }
        public long Time { get; set; }
        public DateTime Date
        {
            get
            {
                return DateTime.FromFileTimeUtc(Time);
            }
        }
    }
}
