using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services.LocalDataServiceUtils.entities
{
    public class MessageDB : Sendable
    {
        [SQLite.PrimaryKey]
        public Guid ID { get; set; }
        public String Text { get; set; }
        public DateTime DateSent { get; set; }
        public Boolean IsSent { get; set; }
        public Boolean IsSendable { get; set; }

    }
}
