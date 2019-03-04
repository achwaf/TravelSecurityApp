using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.DependencyServices
{

    public interface IKeyboardCheck
    {
        event EventHandler KeyboardIsShown;
        event EventHandler KeyboardIsHidden;
    }
}
