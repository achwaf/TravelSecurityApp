using SecurityTravelApp.Views;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Utils
{
    class Utilities
    {

        public static Type TypeOfNavigationTarget(NavigationItemTarget pTarget)
        {
            switch (pTarget)
            {
                case NavigationItemTarget.Home: return typeof(HomePage);
                case NavigationItemTarget.Messages: return typeof(MessagesPage);
                default: return null;
            }
        }
    }
}
