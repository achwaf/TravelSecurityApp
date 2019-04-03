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
        CallService callSrv;

        public PopupTestPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();
            appMngSrv = (AppManagementService)pSrvFactory.getService(ServiceType.AppManagement);
            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);
            locationSrv = (LocationService)pSrvFactory.getService(ServiceType.Location);
            audioSrv = (AudioService)pSrvFactory.getService(ServiceType.Audio);
            callSrv = (CallService)pSrvFactory.getService(ServiceType.Call);

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            callSrv.callNumber("+212660730411");
            var audioFile = audioSrv.recordAudio();
            if (audioFile != null)
            {
                // add reference to file in database

            }
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
            locationSrv.trackUserGeopositionInBackground();
        }

        private void Button5_Clicked(object sender, EventArgs e)
        {
            LocalDataService.setUserTrackingFlag(false);
            locationSrv.stopTrackingUserGeoposition();
        }
    }
}