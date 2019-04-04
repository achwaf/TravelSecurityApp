using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
using SecurityTravelApp.ViewModels;
using SecurityTravelApp.Views.ViewsUtils;
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
    public partial class MessagesPage : ContentPage, Updatable, I18nable
    {
        LocalDataService localDataSrv;

        private double DimMaskOpacity;

        public MessagesPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            NavigationBar.initializeContent(pSrvFactory, pParam);

            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);

            DimMaskOpacity = Utilities.getOnPlatformValue<double>(Application.Current.Resources["DimMaskOpacity"]);

            // tap gesture recognizers
            var tapGestureRecognizerAddIcon = new TapGestureRecognizer();
            AddIcon.GestureRecognizers.Add(tapGestureRecognizerAddIcon);

            var tapGestureRecognizerCheck = new TapGestureRecognizer();
            CheckIcon.GestureRecognizers.Add(tapGestureRecognizerCheck);

            var tapGestureRecognizerSearch = new TapGestureRecognizer();
            SearchIcon.GestureRecognizers.Add(tapGestureRecognizerSearch);

            var tapGestureRecognizerDimMask = new TapGestureRecognizer();
            DimMask.GestureRecognizers.Add(tapGestureRecognizerDimMask);

            var tapGestureRecognizerSend = new TapGestureRecognizer();
            SendIcon.GestureRecognizers.Add(tapGestureRecognizerSend);

            var tapGestureRecognizerCancel = new TapGestureRecognizer();
            CancelIcon.GestureRecognizers.Add(tapGestureRecognizerCancel);

            var tapGestureRecognizerReduce = new TapGestureRecognizer();
            ReduceIcon.GestureRecognizers.Add(tapGestureRecognizerReduce);


            // setting the handler to DimMask
            tapGestureRecognizerDimMask.Tapped += (s, e) =>
            {
                messageEditor.Unfocus();
                DimMask.FadeTo(0, 100);
                MessagingComp.FadeTo(0, 100);
                DimMask.IsVisible = false;
                MessagingComp.IsVisible = false;
            };

            // setting the handler to AddIcon
            tapGestureRecognizerAddIcon.Tapped += (s, e) =>
            {
                messageEditor.Text = String.Empty;
                CancelIcon.IsVisible = false;
                ReduceIcon.IsVisible = true;
                DimMask.Opacity = 0;
                MessagingComp.Opacity = 0;
                MessagingComp.TranslationY = 100;
                DimMask.IsVisible = true;
                MessagingComp.IsVisible = true;
                DimMask.FadeTo(DimMaskOpacity, 100);
                MessagingComp.FadeTo(1, 100);
                MessagingComp.TranslateTo(0, 0, 500, Easing.CubicOut);
            };



            // setting the handler to SendIcon
            tapGestureRecognizerSend.Tapped += (s, e) =>
            {
                // add msg to list
                String msg = messageEditor.Text;
                Message message = new Message(msg, false, "");
                MessageContainer.Children.Insert(0, new MessageComp(message));

                // save to database
                localDataSrv.saveMessage(message);

                // reduce msg composer
                reduceMsgComposer();

                // flush editor
                messageEditor.Text = String.Empty;

                // add msg to sqlite
                // send msg to backend
            };

            // setting the handler to ReduceIcon
            tapGestureRecognizerReduce.Tapped += async (s, e) =>
            {
                reduceMsgComposer();
            };


            // setting the handler to CancelIcon
            tapGestureRecognizerCancel.Tapped += (s, e) =>
            {
                messageEditor.Text = String.Empty;
            };

            // subscribe to editor changes
            messageEditor.TextChanged += MessageEditor_TextChanged;

            // fill predefined data
            populateDefinedMessages(localDataSrv.getDefinedMessages());

            // fill with data
            populate();

            // Text
            updateTXT();

            // subscribe to textUpdates from DefinedMessages
            MessagingCenter.Subscribe<DefinedMessageComp, String>(this, "TEXTUPDATE", (sender, pMessage) =>
                    {
                        // update Text
                        messageEditor.Text = pMessage;
                    });

        }

        public void updateTXT()
        {
            MsgTXT.Text = I18n.GetText(AppTextID.MESSAGES);
            NavigationBar.updateTXT();
            EmptyTXT.Text = I18n.GetText(AppTextID.EMPTY);
        }

        private async void reduceMsgComposer()
        {
            messageEditor.Unfocus();
            await Task.WhenAll(MessagingComp.TranslateTo(0, 100, 500, Easing.CubicOut),
            DimMask.FadeTo(0, 300),
            MessagingComp.FadeTo(0, 300));
            DimMask.IsVisible = false;
            MessagingComp.IsVisible = false;
        }

        private async void MessageEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            // send button effect
            if (String.IsNullOrEmpty(messageEditor.Text))
            {
                SendIcon.FadeTo(.4, 100);
                SendIcon.IsEnabled = false;

            }
            else
            {
                SendIcon.FadeTo(1, 100);
                SendIcon.IsEnabled = true;
            }

            // delete/cancel button effect
            if (CancelIcon.IsVisible && String.IsNullOrEmpty(messageEditor.Text))
            {
                Utilities.switchBetween(CancelIcon, ReduceIcon);
            }
            else if (ReduceIcon.IsVisible && !String.IsNullOrEmpty(messageEditor.Text))
            {
                Utilities.switchBetween(ReduceIcon, CancelIcon);
            }
        }

        private void populateDefinedMessages(List<String> pList)
        {
            foreach (var msg in pList)
            {
                ListMessages.Children.Add(new DefinedMessageComp(msg));
            }
        }


        private void pupulateMessages(List<Message> pList)
        {

            if (pList.Count == 0)
            {
                EmptyInfo.IsVisible = true;
            }
            else
            {
                foreach (var msg in pList)
                {
                    MessageComp alertComp = new MessageComp(msg);
                    MessageContainer.Children.Add(alertComp);
                }
                // adding spacer to be able to scroll up the last elements
                MessageContainer.Children.Add(new BoxView() { HeightRequest = 60 });
            }

        }



        private async void populate()
        {
            // clear
            MessageContainer.Children.Clear();


            // pupulate the stored messages
            var listMessages = await localDataSrv.getListMessage();
            pupulateMessages(listMessages);
        }

        public void update(NavigationParams pParam)
        {
            // update navigation bar
            NavigationBar.update(pParam);
            // update data
            if (!pParam.NavigationBarOnly)
            {
                // probably update only sent status of messages
                // update TXT for children messages , we omit the last because it s just a spacer
                for (int i = 0; i < MessageContainer.Children.Count - 1; i++)
                {
                    View child = MessageContainer.Children[i];
                    MessageComp msg = (MessageComp)child;
                    msg.updateTXT();
                }
            }
        }

        private void KeyboardService_KeyboardIsHidden(object sender, EventArgs e)
        {
            ThePage.Padding = new Thickness(0);
        }

        private void KeyboardService_KeyboardIsShown(object sender, EventArgs e)
        {
            ThePage.Padding = new Thickness(0, 0, 0, 200);
        }

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            //if (PopupNavigation.Instance.PopupStack.Count > 0)
            //{
            //    PopupNavigation.Instance.PopAsync();
            //}
            return true;
        }
    }
}