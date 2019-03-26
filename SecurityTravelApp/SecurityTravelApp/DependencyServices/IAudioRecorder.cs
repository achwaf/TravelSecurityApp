using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.DependencyServices
{
    public interface IAudioRecorder
    {
        String StartRecording();
        void StopRecording();
        void PlayRecording();
    }
}
