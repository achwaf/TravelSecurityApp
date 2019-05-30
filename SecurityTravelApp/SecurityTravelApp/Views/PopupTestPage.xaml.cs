using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Services.LocalDataServiceUtils.entities;
using SecurityTravelApp.Views.Popups;
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
    public partial class PopupTestPage : ContentPage
    {

        AppManagementService appMngSrv;
        LocalDataService localDataSrv;
        AudioService audioSrv;
        LocationService locationSrv;
        LocalNotifService localNotifSrv;
        CallService callSrv;

        public PopupTestPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);
            locationSrv = (LocationService)pSrvFactory.getService(ServiceType.Location);
            audioSrv = (AudioService)pSrvFactory.getService(ServiceType.Audio);
            callSrv = (CallService)pSrvFactory.getService(ServiceType.Call);
            localNotifSrv = (LocalNotifService)pSrvFactory.getService(ServiceType.LocalNotif);

            var tapGestureRecognizer = new TapGestureRecognizer();
            testRoundnessIOS.GestureRecognizers.Add(tapGestureRecognizer);

            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("IOS TEST");
                await testRoundnessIOS.FadeTo(.1, 100);
            };


        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var audioFile = audioSrv.recordAudio();
            if (audioFile != null)
            {
                // add reference to file in database
            }
            callSrv.callNumber("+212642463785");
            //callSrv.callNumber("+212600000000");
        }


        private async void Button2_Clicked(object sender, EventArgs e)
        {
            AudioRecord audio = new AudioRecord()
            {
                audioFile = "file",
                audioLabel = "label",
                isSent = false
            };
            await localDataSrv.saveAudioRecord(audio);
            AudioRecordDB audioDB = await localDataSrv.getAudioRecordDB(audio);
            await localDataSrv.toggleSendable(audioDB, true);

            AudioRecordDB audioDBUpdated = await localDataSrv.getAudioRecordDB(audio);

        }

        private void Button3_Clicked(object sender, EventArgs e)
        {
            appMngSrv.launchTaskSync();
        }

        private void Button4_Clicked(object sender, EventArgs e)
        {
            LocalDataService.setUserTrackingFlag(true);
            localNotifSrv.showNotifTrackingOn();
            locationSrv.trackUserGeopositionInBackground();
        }

        private void Button5_Clicked(object sender, EventArgs e)
        {
            LocalDataService.setUserTrackingFlag(false);
            localNotifSrv.showNotifTrackingOff();
            locationSrv.stopTrackingUserGeoposition();
            locationSrv.disableBackgroundLocationUpdates();
        }

        private void Button6_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new NotifAlertPopup());
        }
    }
}