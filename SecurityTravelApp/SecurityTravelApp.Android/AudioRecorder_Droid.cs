using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SecurityTravelApp.DependencyServices;
using SecurityTravelApp.Droid;
using SecurityTravelApp.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioRecorder_Droid))]
namespace SecurityTravelApp.Droid
{
    class AudioRecorder_Droid : IAudioRecorder
    {
        static long Time { get; set; }
        MediaRecorder _recorder;
        MediaPlayer _player;
        //string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "test");
        private static String Storage = Android.OS.Environment.DirectoryDocuments;
        private static String AudioFolder = "TravelSecurity";
        private String audioPath;

        public AudioRecorder_Droid()
        {
            checkFolderExists(Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Storage).AbsolutePath, AudioFolder));
            _recorder = new MediaRecorder();
            _player = new MediaPlayer();

            _player.Completion += (sender, e) =>
            {
                _player.Reset();
            };
        }

        private void checkFolderExists(String pPath)
        {
            if (!Directory.Exists(pPath))
            {
                Directory.CreateDirectory(pPath);
            }
        }

        public String StartRecording()
        {

            // the recorder overrides the files if it exists, and create it if not
            try
            {
                // setting the file name with current date
                String fileName = "SOS_" + DateTime.Now.ToString(Constantes.DATEFORMAT_RECORD_AUDIO) + Constantes.RECORD_AUDIO_FILE_EXTENSION;
                audioPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Storage).AbsolutePath, AudioFolder, fileName);


                _recorder.SetAudioSource(AudioSource.Mic);
                _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                _recorder.SetAudioEncoder(AudioEncoder.AmrWb);
                _recorder.SetMaxDuration(60000); // 60 seconds
                _recorder.SetOutputFile(audioPath);
                _recorder.Prepare();
                _recorder.Start();
            }
            catch (Exception e)
            {
                // if exception the file will not be created
                var str = e.Message;
                return null;
            }

            // if no exception, return the path
            return audioPath;

        }

        public void StopRecording()
        {
            try
            {
                _recorder.Stop();
                _recorder.Reset();
            }
            catch (Exception e)
            {
                var str = e.Message;
            }
        }

        public async void PlayRecording()
        {
            try
            {
                _player.Prepare();
                Time = _player.Duration;
                _player.Looping = false;
                _player.Start();
            }
            catch (Exception e)
            {
                var str = e.Message;
            }
        }
    }
}