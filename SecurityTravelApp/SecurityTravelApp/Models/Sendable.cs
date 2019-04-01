using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Models
{
    public interface Sendable : Matchable
    {
        Boolean IsSendable { get; set; }
    }
}
