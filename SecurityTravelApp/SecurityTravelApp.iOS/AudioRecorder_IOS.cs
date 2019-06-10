using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AVFoundation;
using Foundation;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioRecorder_IOS))]
namespace SecurityTravelApp.iOS
{
    class AudioRecorder_IOS : IAudioRecorder
    {

        AVAudioRecorder _recorder;
        AVAudioPlayer _player;
        private static String Storage = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static String AudioFolder = "TravelSecurity";
        private String audioPath;

        public AudioRecorder_IOS()
        {
            checkFolderExists(Path.Combine(Storage, AudioFolder));
            _recorder = new AVAudioRecorder();
        }

        public void PlayRecording()
        {
            throw new NotImplementedException();
        }

        public string StartRecording()
        {
            throw new NotImplementedException();
        }

        public void StopRecording()
        {
            throw new NotImplementedException();
        }

        private void checkFolderExists(String pPath)
        {
            if (!Directory.Exists(pPath))
            {
                Directory.CreateDirectory(pPath);
            }
        }
    }
}