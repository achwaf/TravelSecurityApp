using SecurityTravelApp.DependencyServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

        public async Task<Boolean> sendSMSAsync(String pSMS)
        {
            try
            {
                var message = new SmsMessage(pSMS, new[] { "+212600000000" });
                await Sms.ComposeAsync(message);
                return true;
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
                return false;
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                return false;
            }
        }
    }
}
