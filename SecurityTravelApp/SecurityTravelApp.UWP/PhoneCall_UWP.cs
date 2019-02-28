using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.UWP;
using Xamarin.Forms;


[assembly: Dependency(typeof(PhoneCall_UWP))]
namespace SecurityTravelApp.UWP
{
    class PhoneCall_UWP : IPhoneCall
    {
        public void makePhoneCall(string pNumber)
        {
            // make phone call UWP
        }
    }
}
