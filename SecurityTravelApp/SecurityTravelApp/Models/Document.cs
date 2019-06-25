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
        public String data;
        public DateTime dateReceived;
        public Boolean isSeen;
        public DateTime dateSeen;
        public String fileExtension;
        public DocumentType type;

        public Document()
        {
            ID = Guid.NewGuid();
        }

        public Document(DocumentType pType, String pRegion, String pTitle, String pDescription, String pData, String pExt, String pRecieved, String pSeen) : this()
        {
            title = pTitle;
            type = pType;
            data = pData;
            description = pDescription;
            region = pRegion;
            fileExtension = pExt;
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
        PDF,
        Word,
        Excel,
        Image,
        PowerPoint,
        Audio,
        Video,
        Other
    }
}
