using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class Document : Matchable
    {
        public Guid ID { get; set; }
        public String title;
        public String region;
        public String description;
        public String text;
        public DateTime dateReceived;
        public Boolean isSeen;
        public DateTime dateSeen;
        public DocumentType type;

        public Document()
        {
            ID = Guid.NewGuid();
        }

        public Document(DocumentType pType, String pRegion, String pTitle, String pDescription, String pText, String pRecieved, String pSeen) : this()
        {
            title = pTitle;
            type = pType;
            text = pText;
            description = pDescription;
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

    public enum DocumentType
    {
        Text,
        Link,
        Downloadable
    }
}
