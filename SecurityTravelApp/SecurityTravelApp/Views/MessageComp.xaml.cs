using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Models;
using SecurityTravelApp.Utils;
using SecurityTravelApp.Views.Popups;
using SecurityTravelApp.Views.ViewsUtils;
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
    public partial class MessageComp : ContentView, I18nable
    {
        private Boolean continueFlashAnimation;
        private Animation flashingAnimation;
        private Animation HidingAnimation;
        private Message theMessage;

        public MessageComp(Message pMessage)
        {
            InitializeComponent();
            theMessage = pMessage;
            this.BindingContext = theMessage;

            // setting the taphandler for the MessageComp
            var tapGestureRecognizer = new TapGestureRecognizer();

            // setting the tapHandler for the SMS action
            var tapGestureRecognizerSMS = new TapGestureRecognizer();

            if (!pMessage.isSent)
            {


                this.GestureRecognizers.Add(tapGestureRecognizer);
                tapGestureRecognizer.Tapped += async (s, e) =>
                {
                    if (ActionPanel.IsVisible == true)
                    {
                        await this.FadeTo(.5, 50);
                        ActionPanel.IsVisible = false;
                        SelectionMask.IsVisible = false;
                        ViewExtensions.CancelAnimations(ActionPanel);
                        this.FadeTo(1, 500);
                    }
                    else
                    {
                        await this.FadeTo(.5, 50);
                        ActionPanel.Opacity = 1;
                        ActionPanel.IsVisible = true;
                        SelectionMask.IsVisible = true;
                        this.FadeTo(1, 500);
                        SlowHidingAnimation();
                    }
                };

                ActionPanel.GestureRecognizers.Add(tapGestureRecognizerSMS);
                tapGestureRecognizerSMS.Tapped += async (s, e) =>
                {
                    await SendSMSActionMask.FadeTo(.8, 80);
                    ViewExtensions.CancelAnimations(ActionPanel);
                    ActionPanel.Opacity = 1;


                    // update waiting text to SMS
                    Waiting.Text = I18n.GetText(AppTextID.WAITING_FOR_TRANSFER_SMS);

                    // send via SMS
                    MessagingCenter.Send<MessageComp, Message>(this, "MSGSMSTOSEND", pMessage);

                    await SendSMSActionMask.FadeTo(0, 120);
                    ActionPanel.IsVisible = false;
                    SelectionMask.IsVisible = false;
                };
            }

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

        private void SlowHidingAnimation()
        {
            // animation to hide the actionPanel after some time 
            HidingAnimation = new Animation();
            var fadeoutActionAnimation = new Animation(v =>
            {
                ActionPanel.Opacity = v;
                if (v == 0)
                {
                    ActionPanel.IsVisible = false;
                    SelectionMask.IsVisible = false;
                }
            }, 1, 0);
            HidingAnimation.Add(.75, 1, fadeoutActionAnimation);
            HidingAnimation.Commit(this, "hiding", 16, 3000);
        }

        public void updateTXT()
        {
            DateSent.Text = theMessage.DateString;
            Waiting.Text = theMessage.WaitingString;
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