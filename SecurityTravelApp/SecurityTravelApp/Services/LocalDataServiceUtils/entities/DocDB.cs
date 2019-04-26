﻿using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services.LocalDataServiceUtils.entities
{
    public class DocDB : Matchable
    {
        [SQLite.PrimaryKey]
        public Guid ID { get; set; }
        public String Title { get; set; }
        public String Text { get; set; }
        public DateTime DateReceived { get; set; }
        public Boolean IsSeen { get; set; }
        public DateTime DateSeen { get; set; }
    }
}
