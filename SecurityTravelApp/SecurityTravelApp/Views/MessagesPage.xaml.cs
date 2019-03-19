using SecurityTravelApp.DependencyServices;
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
    public partial class MessagesPage : ContentPage, UpdatablePage
    {
        IKeyboardCheck keyboardService;
        LocalDataService localDataSrv;

        private double DimMaskOpacity;

        public MessagesPage(ServiceFactory pSrvFactory, AfterNavigationParams pParam)
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
            };

            // setting the handler to ReduceIcon
            tapGestureRecognizerReduce.Tapped += async (s, e) =>
            {
                messageEditor.Unfocus();
                await Task.WhenAll(MessagingComp.TranslateTo(0, 100, 500, Easing.CubicOut),
                DimMask.FadeTo(0, 300),
                MessagingComp.FadeTo(0, 300));
                DimMask.IsVisible = false;
                MessagingComp.IsVisible = false;
            };


            // setting the handler to CancelIcon
            tapGestureRecognizerCancel.Tapped += (s, e) =>
            {
                messageEditor.Text = String.Empty;
            };

            // subscribe to editor changes
            messageEditor.TextChanged += MessageEditor_TextChanged;

            // populate the defined messages
            populateDefinedMessages(localDataSrv.getDefinedMessages());

            // subscribe to textUpdates from DefinedMessages
            MessagingCenter.Subscribe<DefinedMessageComp, String>(this, "TEXTUPDATE", (sender, pMessage) =>
                    {
                        // update Text
                        messageEditor.Text = pMessage;
                    });

        }

        private async void MessageEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
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


        public void update()
        {
        }

        private void KeyboardService_KeyboardIsHidden(object sender, EventArgs e)
        {
            ThePage.Padding = new Thickness(0);
        }

        private void KeyboardService_KeyboardIsShown(object sender, EventArgs e)
        {
            ThePage.Padding = new Thickness(0, 0, 0, 200);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //keyboardService.KeyboardIsShown += KeyboardService_KeyboardIsShown;
            //keyboardService.KeyboardIsHidden += KeyboardService_KeyboardIsHidden;

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //keyboardService.KeyboardIsShown -= KeyboardService_KeyboardIsShown;
            //keyboardService.KeyboardIsHidden -= KeyboardService_KeyboardIsHidden;
        }
    }
}