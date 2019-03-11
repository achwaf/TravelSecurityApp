using SecurityTravelApp.DependencyServices;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SecurityTravelApp.Services
{
    class CallService : ServiceAbstract
    {
        const ServiceType TYPE = ServiceType.Call;
        private IPhoneCall phoneCaller;


        public CallService() : base(TYPE)
        {
            phoneCaller = DependencyService.Get<IPhoneCall>();

        }

        public void config()
        {

        }

        public void callNumber(String pNumber)
        {
            phoneCaller.makePhoneCall(pNumber);
        }
    }
}
