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
    public partial class MessageComp : ContentView
    {
        public MessageComp()
        {
            InitializeComponent();

            // setting the taphandler   
            var tapGestureRecognizer = new TapGestureRecognizer();
            this.GestureRecognizers.Add(tapGestureRecognizer);

            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                await this.FadeTo(.5, 50);
                this.FadeTo(1, 500);
            };


        }
    }
}