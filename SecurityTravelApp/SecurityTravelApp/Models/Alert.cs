using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class Alert
    {
        public String title;
        public String text;
        public DateTime dateReceived;
        public Boolean isRead;
        public AlertType type;

        public Alert(AlertType pType, String pTitle, String pText)
        {
            title = pTitle;
            type = pType;
            text = pText;
        }

    }

    public enum AlertType
    {
        Normal,
        Important,
        Critical
    }

}
