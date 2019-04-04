using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class Alert : Matchable
    {
        public Guid ID { get; set; }
        public String title;
        public String region;
        public String text;
        public DateTime dateReceived;
        public Boolean isSeen;
        public DateTime dateSeen;
        public AlertType type;

        public Alert()
        {
            ID = Guid.NewGuid();
        }

        public Alert(AlertType pType, String pRegion, String pTitle, String pText, String pRecieved, String pSeen) : this()
        {
            title = pTitle;
            type = pType;
            text = pText;
            region = pRegion;
            dateReceived = DateTime.Parse(pRecieved);
            if (!String.IsNullOrEmpty(pSeen))
            {
                isSeen = true;
                dateSeen = DateTime.Parse(pSeen);
            }
            else
            {
                isSeen = false;
            }
        }

    }

    public enum AlertType
    {
        Normal,
        Important,
        Critical
    }

}
