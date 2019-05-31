using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Services.LocalDataServiceUtils.entities;
using SecurityTravelApp.Utils;
using SecurityTravelApp.ViewModels;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesPage : ContentPage, Updatable, I18nable
    {
        LocalDataService localDataSrv;
        CallService callSrv;


        private List<Message> listMessages;
        private List<Message> researchListMessages;
        private List<Message> activeListMessages;
        private int numberVisibleMessages = 20;
        private int numberRotatableMessages = 10;
        private int firstIndexVisible;
        private int lastIndexVisible;
        private Grid listStartButton;
        private Grid listEndButton;
        private Timer timer;
        private char[] splitArray = { ' ' };
        private double DimMaskOpacity;

        public MessagesPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            NavigationBar.initializeContent(pSrvFactory, pParam);

            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);
            callSrv = (CallService)pSrvFactory.getService(ServiceType.Call);

            DimMaskOpacity = Utilities.getOnPlatformValue<double>(Application.Current.Resources["DimMaskOpacity"]);


            // create list Start and End Button
            createListButtons();

            // tap gesture recognizers
            var tapGestureRecognizerAddIcon = new TapGestureRecognizer();
            AddOrCloseTap.GestureRecognizers.Add(tapGestureRecognizerAddIcon);

            var tapGestureRecognizerSearch = new TapGestureRecognizer();
            SearchTap.GestureRecognizers.Add(tapGestureRecognizerSearch);

            var tapGestureRecognizerDimMask = new TapGestureRecognizer();
            DimMask.GestureRecognizers.Add(tapGestureRecognizerDimMask);

            var tapGestureRecognizerSend = new TapGestureRecognizer();
            SendIcon.GestureRecognizers.Add(tapGestureRecognizerSend);

            var tapGestureRecognizerCancel = new TapGestureRecognizer();
            CancelIcon.GestureRecognizers.Add(tapGestureRecognizerCancel);

            var tapGestureRecognizerReduce = new TapGestureRecognizer();
            ReduceIcon.GestureRecognizers.Add(tapGestureRecognizerReduce);

            var tapGestureDisplayMoreStart = new TapGestureRecognizer();
            listStartButton.GestureRecognizers.Add(tapGestureDisplayMoreStart);

            var tapGestureDisplayMoreEnd = new TapGestureRecognizer();
            listEndButton.GestureRecognizers.Add(tapGestureDisplayMoreEnd);


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
            tapGestureRecognizerAddIcon.Tapped += async (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("IOS TEST TAP");
                await AddOrCloseTap.FadeTo(.1, 100);

                if (SearchEntry.IsVisible)
                {
                    SearchEntry.Text = String.Empty;
                    await SearchBg.FadeTo(0, 80);
                    MsgTXT.FadeTo(1, 80);
                    SearchEntry.IsVisible = false;
                    SearchBg.IsVisible = false;
                    SearchTap.IsEnabled = true;
                    AddIcon.Text = LineAwesomeIcons.Plus;
                    activeListMessages = listMessages;
                    populateMessages(activeListMessages);
                }
                else
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
                }


                AddOrCloseTap.FadeTo(.02, 100);
            };

            // setting the handler to SearchIcon
            tapGestureRecognizerSearch.Tapped += async (s, e) =>
            {
                SearchTap.FadeTo(.1, 100);
                await MsgTXT.FadeTo(0, 80);
                SearchEntry.Text = String.Empty;
                SearchBg.Opacity = 0;
                SearchEntry.IsVisible = true;
                await SearchBg.FadeTo(1, 80);
                SearchBg.IsVisible = true;
                SearchTap.IsEnabled = false;
                AddIcon.Text = LineAwesomeIcons.AngleRight;
                SearchEntry.Focus();
                SearchTap.FadeTo(.02, 100);
            };


            MessagingComp.SizeChanged += MessagingComp_SizeChanged;



            // setting the handler to SendIcon
            tapGestureRecognizerSend.Tapped += (s, e) =>
            {
                // add msg to list
                String msg = messageEditor.Text;
                Message message = new Message(msg, false, "");
                activeListMessages.Insert(0, message);
                populateMessages(activeListMessages);

                // save to database
                localDataSrv.saveMessage(message);

                // reduce msg composer
                reduceMsgComposer();

                // flush editor
                messageEditor.Text = String.Empty;

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

            // setting the handler to List Start Button
            tapGestureDisplayMoreStart.Tapped += (s, e) =>
            {
                int oldFirstIndexVisible = firstIndexVisible;
                int newFirstIndexVisible = firstIndexVisible - numberRotatableMessages;

                if (newFirstIndexVisible < 0)
                {
                    firstIndexVisible = 0;
                    lastIndexVisible = firstIndexVisible + numberVisibleMessages - 1;
                }
                else
                {
                    firstIndexVisible = newFirstIndexVisible;
                    lastIndexVisible = firstIndexVisible + numberVisibleMessages - 1;
                }

                populateMessages(activeListMessages, false);
            };


            // setting the handler to List End Button
            tapGestureDisplayMoreEnd.Tapped += (s, e) =>
            {
                int oldLastIndexVisible = lastIndexVisible;
                int newLastIndexVisible = lastIndexVisible + numberRotatableMessages;

                if (newLastIndexVisible > activeListMessages.Count - 1)
                {
                    lastIndexVisible = activeListMessages.Count;
                    firstIndexVisible = lastIndexVisible - numberVisibleMessages + 1;
                }
                else
                {
                    lastIndexVisible = newLastIndexVisible;
                    firstIndexVisible = lastIndexVisible - numberVisibleMessages + 1;
                }

                populateMessages(activeListMessages, false);

            };

            // subscribe to Search entry
            timer = new Timer();
            timer.Elapsed += searchTimer_Elapsed; ;
            timer.Interval = 500;
            timer.AutoReset = false;
            SearchEntry.TextChanged += SearchEntry_TextChanged;

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


            // subscribe to MSGSMSTOSEND from MessageComp to send the message via SMS
            MessagingCenter.Subscribe<MessageComp, Message>(this, "MSGSMSTOSEND", async (sender, pMessage) =>
                    {
                        // send SMS
                        var sentOK = await callSrv.sendSMSAsync(pMessage.text);

                        if (sentOK)
                        {
                            // update localdata
                            MessageDB msgDB = await localDataSrv.getMessageDB(pMessage);
                            msgDB.DateSent = DateTime.Now;
                            msgDB.IsSent = true;
                            await localDataSrv.updateToDB(msgDB);
                        }

                        populate();
                    });

        }

        private void searchTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            // perform search
            var searchValues = SearchEntry.Text.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);

            if (searchValues.Length == 0)
            {
                activeListMessages = listMessages;
            }
            else
            {
                researchListMessages = new List<Message>();
                foreach (var msg in listMessages)
                {
                    var lowerCaseText = msg.text.ToLower();
                    if (searchValues.Any(value => lowerCaseText.Contains(value)))
                    {
                        researchListMessages.Add(msg);
                    }
                }
                activeListMessages = researchListMessages;
            }


            Device.BeginInvokeOnMainThread(() =>
            {
                populateMessages(activeListMessages);
            });
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            // in order to not perform search while the user is taping text
            // rely on timer before search is done
            if (timer.Enabled)
            {
                timer.Stop();
            }
            timer.Start();
        }

        private void createListButtons()
        {
            // creating button for message display in the list
            listStartButton = new Grid();
            listStartButton.BackgroundColor = Color.WhiteSmoke;
            listStartButton.HorizontalOptions = LayoutOptions.Fill;
            listStartButton.VerticalOptions = LayoutOptions.Fill;
            var labelStart = new Label();
            labelStart.Text = "Display More";
            labelStart.TextColor = Color.LightGray;
            labelStart.Margin = new Thickness(10);
            labelStart.HorizontalTextAlignment = TextAlignment.Center;
            labelStart.VerticalTextAlignment = TextAlignment.Center;
            labelStart.VerticalOptions = LayoutOptions.Center;
            labelStart.HorizontalOptions = LayoutOptions.Center;
            listStartButton.Children.Add(labelStart);

            // creating button for message display at the end of the list
            listEndButton = new Grid();
            listEndButton.BackgroundColor = Color.WhiteSmoke;
            listEndButton.HorizontalOptions = LayoutOptions.Fill;
            listEndButton.VerticalOptions = LayoutOptions.Fill;
            var labelEnd = new Label();
            labelEnd.Text = "Display More";
            labelEnd.TextColor = Color.LightGray;
            labelEnd.Margin = new Thickness(10);
            labelEnd.HorizontalTextAlignment = TextAlignment.Center;
            labelEnd.VerticalTextAlignment = TextAlignment.Center;
            labelEnd.VerticalOptions = LayoutOptions.Center;
            labelEnd.HorizontalOptions = LayoutOptions.Center;
            listEndButton.Children.Add(labelEnd);

        }

        private void MessagingComp_SizeChanged(object sender, EventArgs e)
        {
            messageEditor.WidthRequest = MessagingComp.Width - 60;
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

        private void MessageEditor_TextChanged(object sender, TextChangedEventArgs e)
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


        private void populateMessages(List<Message> pList, Boolean pFirstDisplay = true)
        {
            // clear
            MessageContainer.Children.Clear();

            if (pList.Count == 0)
            {
                EmptyInfo.IsVisible = true;
            }
            else
            {
                EmptyInfo.IsVisible = false;
                if (pFirstDisplay)
                {
                    // init first and end indexes 
                    if (pList.Count > numberVisibleMessages - 1) // fill the visible list
                    {
                        firstIndexVisible = 0;
                        lastIndexVisible = numberVisibleMessages - 1;
                    }
                    else
                    {
                        firstIndexVisible = 0;
                        lastIndexVisible = pList.Count;
                    }
                }

                var rangeMessages = lastIndexVisible - firstIndexVisible + 1;
                var listVisibleMessages = pList.Skip(firstIndexVisible).Take(rangeMessages).ToList();
                if (firstIndexVisible > 0)
                {
                    // show the top button for displaying more
                    MessageContainer.Children.Add(listStartButton);
                }
                foreach (var msg in listVisibleMessages)
                {
                    MessageComp alertComp = new MessageComp(msg);
                    MessageContainer.Children.Add(alertComp);
                }

                if (lastIndexVisible < pList.Count - 1)
                {
                    // show the End button for displaying more
                    MessageContainer.Children.Add(listEndButton);
                }

                // adding spacer to be able to scroll up the last elements
                MessageContainer.Children.Add(new BoxView() { HeightRequest = 60 });

                if (pFirstDisplay)
                {
                    ScrollMsg.ScrollToAsync(0, 0, true);
                }
            }

        }



        private async void populate()
        {
            // clear 
            MessageContainer.Children.Clear();

            // pupulate the stored messages
            listMessages = await localDataSrv.getListMessage(); // get the stored messages
            activeListMessages = listMessages;

            populateMessages(activeListMessages);
        }

        public void update(NavigationParams pParam)
        {
            // update navigation bar
            NavigationBar.update(pParam);
            // update data
            if (!pParam.NavigationBarOnly)
            {
                // probably update only sent status of messages
                // maybe update directly in the list , or compare between database retrieved one and the current one
                // or at final thought, better populate from scratch since suppression was added
                populate();

            }
        }

        public void updateTXT()
        {
            MsgTXT.Text = I18n.GetText(AppTextID.MESSAGES);
            NavigationBar.updateTXT();
            EmptyTXT.Text = I18n.GetText(AppTextID.EMPTY);

            // update TXT for children messages , we omit the last because it s just a spacer
            for (int i = 0; i < MessageContainer.Children.Count - 1; i++)
            {
                View child = MessageContainer.Children[i];
                if (child is MessageComp)
                {
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