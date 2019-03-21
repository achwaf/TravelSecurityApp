using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageComp : ContentView
    {
        private Boolean continueFlashAnimation;
        private Animation flashingAnimation;
        private Message theMessage;

        public MessageComp(Message pMessage)
        {
            InitializeComponent();
            theMessage = pMessage;
            this.BindingContext = theMessage;
            // setting the taphandler   
            var tapGestureRecognizer = new TapGestureRecognizer();
            this.GestureRecognizers.Add(tapGestureRecognizer);

            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                await this.FadeTo(.5, 50);
                this.FadeTo(1, 500);
            };

            // animation pending msg to be sent
            flashingAnimation = new Animation();
            var fadeinAnimation = new Animation(v => PendingIcon.Opacity = v, 0, 1);
            var fadeoutAnimation = new Animation(v => PendingIcon.Opacity = v, 1, 0);
            flashingAnimation.Add(0, .2, fadeinAnimation);
            flashingAnimation.Add(.8, 1, fadeoutAnimation);

            // launch the animation when pending icon is visible
            INotifyPropertyChanged observableMessage = (INotifyPropertyChanged)theMessage;
            observableMessage.PropertyChanged += ObservableMessage_PropertyChanged;


            // check animationPendingIcon for first display
            checkAnimationPendingIcon();

        }

        private void checkAnimationPendingIcon()
        {
            if (!theMessage.isSent)
            {
                PendingIcon.Opacity = 0;
                continueFlashAnimation = true;
                flashingAnimation.Commit(this, "pending", 16, 500, null, repeat: () => { return continueFlashAnimation; });
            }
            else
            {
                continueFlashAnimation = false;
                ViewExtensions.CancelAnimations(PendingIcon);
            }
        }

        private void ObservableMessage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("isSent"))
            {
                checkAnimationPendingIcon();
            }
        }
    }
}