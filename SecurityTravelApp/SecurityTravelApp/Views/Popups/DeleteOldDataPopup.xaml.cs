using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteOldDataPopup : Rg.Plugins.Popup.Pages.PopupPage, I18nable
    {

        private List<TimePickerItem> ListValuesPicker = new List<TimePickerItem>();
        private LocalDataService localDataSrv;

        private Boolean DeleteButtonTapped;
        public DeleteOldDataPopup(ServiceFactory pSrvFactory)
        {
            initValuesPicker();
            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);



            InitializeComponent();

            // add tap_handler for the backIcon button
            var tapGestureRecognizerActionButton = new TapGestureRecognizer();
            tapGestureRecognizerActionButton.Tapped += async (s, e) =>
            {
                if (!DeleteButtonTapped)
                {
                    DeleteButtonTapped = true;
                    // dsplay confirmation
                    ConfirmTxt.Opacity = 0;
                    ConfirmTxt.TranslationX = -50;
                    ConfirmTxt.IsVisible = true;

                    ButtonAction.IsEnabled = false;
                    await ButtonAction.FadeTo(.2, 80);
                    ButtonAction.Text = I18n.GetText(AppTextID.CONFIRM);
                    ButtonAction.TextColor = Color.DarkRed;

                    ButtonAction.FadeTo(1, 80);
                    ConfirmTxt.FadeTo(.6, 80);
                    ConfirmTxt.TranslateTo(0, 0, 80, Easing.CubicOut);
                    ButtonAction.IsEnabled = true;

                }
                else
                {
                    // perform data suppression
                    TimePickerItem item = (TimePickerItem)TimePicker.SelectedItem;
                    if (item != null)
                    {
                        // UX before suppression
                        ButtonAction.FadeTo(.2, 80);
                        await ConfirmTxt.FadeTo(0, 80);
                        ConfirmTxt.IsVisible = false;
                        ButtonAction.Text = I18n.GetText(AppTextID.CLEANING);
                        ButtonAction.TextColor = Color.Gray;
                        ButtonAction.IsEnabled = false;
                        ButtonAction.FadeTo(1, 80);
                        TimePicker.IsEnabled = false;

                        var pLimitDate = DateTime.Now.Add(-item.timeSpan);
                        await localDataSrv.deleteDataOlderThan(pLimitDate);

                        // UX after suppression
                        ButtonAction.FadeTo(0, 80);
                        TagIcon.FadeTo(0, 160);
                        await TagIcon.TranslateTo(0, 50, 160, Easing.CubicIn);
                        TagBg.Color = Color.Green;
                        TagIcon.TextColor = Color.DarkGreen;
                        TagIcon.Text = LineAwesomeIcons.Check;
                        TagIcon.FadeTo(1, 160);
                        TagIcon.TranslateTo(0, 0, 160, Easing.CubicOut);

                    }
                }
            };
            ButtonAction.GestureRecognizers.Add(tapGestureRecognizerActionButton);
            TimePicker.ItemsSource = ListValuesPicker;
            TimePicker.SelectedIndex = 0;
            TimePicker.SelectedIndexChanged += TimePicker_SelectedIndexChanged;

            // display info on data
            var Item = (TimePickerItem)TimePicker.SelectedItem;
            displayHowMuchDataToDelete(Item.timeSpan);
            displayTotalData();

            updateTXT();
        }

        private async void displayHowMuchDataToDelete(TimeSpan pTimeSpan)
        {
            int intAlertCount, intMsgCount, intLocationCount, intDocCount, intAudioCount;
            var limitDate = DateTime.Now.Add(-pTimeSpan);

            intAlertCount = await localDataSrv.getCountAlertOlderThan(limitDate);
            AlertesToDelete.Text = intAlertCount.ToString();

            intMsgCount = await localDataSrv.getCountMsgOlderThan(limitDate);
            MessagesToDelete.Text = intMsgCount.ToString();

            intLocationCount = await localDataSrv.getCountLocationOlderThan(limitDate);
            LocationsToDelete.Text = intLocationCount.ToString();

            intDocCount = await localDataSrv.getCountDocOlderThan(limitDate);
            DocsToDelete.Text = intDocCount.ToString();

            intAudioCount = await localDataSrv.getCountAudioOlderThan(limitDate);
            AudiosToDelete.Text = intAudioCount.ToString();

        }



        private async void displayTotalData()
        {
            int intAlertCount, intMsgCount, intLocationCount, intDocCount, intAudioCount;

            intAlertCount = await localDataSrv.getListAlertCount();
            TotalAlertes.Text = "/ " + intAlertCount;

            intMsgCount = await localDataSrv.getListMsgCount();
            TotalMessages.Text = "/ " + intMsgCount;

            intLocationCount = await localDataSrv.getListLocationCount();
            TotalLocations.Text = "/ " + intLocationCount;

            intDocCount = await localDataSrv.getListDocCount();
            TotalDocs.Text = "/ " + intDocCount;

            intAudioCount = await localDataSrv.getListAudioCount();
            TotalAudios.Text = "/ " + intAudioCount;

        }

        private void initValuesPicker()
        {
            //ListValuesPicker.Add(new TimePickerItem(TimeSpan.FromDays(14), I18n.GetText(AppTextID._2WEEKS)));
            ListValuesPicker.Add(new TimePickerItem(TimeSpan.FromDays(30), I18n.GetText(AppTextID._1MONTH)));
            ListValuesPicker.Add(new TimePickerItem(TimeSpan.FromDays(91), I18n.GetText(AppTextID._3MONTHS)));
            ListValuesPicker.Add(new TimePickerItem(TimeSpan.FromDays(183), I18n.GetText(AppTextID._6MONTHS)));
            ListValuesPicker.Add(new TimePickerItem(TimeSpan.FromDays(365), I18n.GetText(AppTextID._1YEAR)));
        }

        private void TimePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TimePicker.SelectedIndex > -1)
            {
                var Item = (TimePickerItem)TimePicker.SelectedItem;
                displayHowMuchDataToDelete(Item.timeSpan);
            }
        }

        public void updateTXT()
        {
            ButtonAction.Text = I18n.GetText(AppTextID.DELETE);
            ConfirmTxt.Text = I18n.GetText(AppTextID.AREYOUSURE);
            DeleteTexte.Text = I18n.GetText(AppTextID.DELETE_DATA_OLDER_THAN);
        }
    }

    class TimePickerItem
    {
        public TimeSpan timeSpan;
        public String text { get; set; }

        public TimePickerItem(TimeSpan pTimeSpan, String pText)
        {
            timeSpan = pTimeSpan;
            text = pText;
        }
    }
}