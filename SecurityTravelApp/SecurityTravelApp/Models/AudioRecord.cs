using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public class AudioRecord : Matchable
    {
        public Guid ID { get; set; }
        public String audioFile;
        public Boolean isSent;
        public DateTime dateSent;
        public DateTime dateRecorded;
        public String audioLabel;

        public AudioRecord() { ID = Guid.NewGuid(); }
    }
}
