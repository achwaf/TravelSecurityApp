using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SecurityTravelApp.Services
{
    class AudioService : ServiceAbstract
    {
        const ServiceType TYPE = ServiceType.Audio;
        private IAudioRecorder recorder;
        private Boolean isRecording = false;

        public AudioService() : base(TYPE)
        {
            recorder = DependencyService.Get<IAudioRecorder>();

        }



        public void config()
        {

        }

        public async Task<String> recordAudio()
        {
            String audioPath = null;
            if (await PermissionChecker.checkForPermission(Plugin.Permissions.Abstractions.Permission.Microphone)
                && await PermissionChecker.checkForPermission(Plugin.Permissions.Abstractions.Permission.Storage))
            {
                if (!isRecording)
                {
                    isRecording = true;
                    audioPath = recorder.StartRecording();

                    if (audioPath != null)
                    {
                        // stop the recording after a few seconds
                        Device.StartTimer(TimeSpan.FromSeconds(60), () =>
                        {
                            recorder.StopRecording();
                            return true;
                        });
                        isRecording = false;
                    }
                    else
                    {
                        isRecording = false;
                    }
                }
            }

            return audioPath;
        }
    }

}
