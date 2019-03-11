using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefinedMessageComp : ContentView
    {
        public DefinedMessageComp(String pMessage)
        {
            InitializeComponent();
            Message.Text = pMessage;


            // tap gesture recognizers
            var tapGestureRecognizer = new TapGestureRecognizer();
            this.GestureRecognizers.Add(tapGestureRecognizer);

            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                await Highlight.FadeTo(.5, 50);
                MessagingCenter.Send<DefinedMessageComp, string>(this, "TEXTUPDATE", pMessage);
                Highlight.FadeTo(.2, 500);
            };


        }


    }
}