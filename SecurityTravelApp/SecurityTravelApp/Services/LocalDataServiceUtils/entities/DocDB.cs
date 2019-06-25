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
        public String Region { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Data { get; set; }
        public DateTime DateReceived { get; set; }
        public Boolean IsSeen { get; set; }
        public DateTime DateSeen { get; set; }
        public String FileExtension { get; set; }
        public DocumentType Type { get; set; }
    }
}
