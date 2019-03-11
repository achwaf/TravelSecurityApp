using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services
{
    class AudioService : ServiceAbstract
    {
        const ServiceType TYPE = ServiceType.Audio;

        public AudioService() : base(TYPE)
        {
        }

        public void config()
        {

        }

    }
}
