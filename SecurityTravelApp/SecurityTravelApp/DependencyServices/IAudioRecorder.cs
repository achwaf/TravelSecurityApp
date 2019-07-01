using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTravelApp.DependencyServices
{
    public interface IAudioRecorder
    {
        Task<String> StartRecording();
        void StopRecording();
        void PlayRecording();
    }
}
