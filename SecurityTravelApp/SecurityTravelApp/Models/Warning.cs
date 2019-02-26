using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class Warning
    {
        public String title;
        public String text;
        public String dateReceived;
        public Boolean isRead;

    }

    public enum WarningType
    {
        Normal,
        Important,
        Critical
    }

}
