using UnityEngine;

namespace SMILEI.Vokaturi
{
    public class MicrophoneToBuffer : MonoBehaviour
    {
        public AudioBuffer WriteBuffer;
        public string DeviceName;
        public int Frequency;
        public int LengthInSeconds = 1;
    
        private string _deviceName => string.IsNullOrEmpty(DeviceName) ? null : DeviceName;
        private void OnEnable() => StartListening();
        private void OnDisable() => StopListening();
    

        [ContextMenu("Start Listening")]
        void StartListening()
        {
            if (WriteBuffer == null) return;
            //WriteBuffer.InitializeBufferIfNeeded(LengthInSeconds, 1, Frequency); // Microphone.Start creates audioclip
            WriteBuffer.Buffer = Microphone.Start(_deviceName, true, LengthInSeconds, Frequency);
            WriteBuffer.Buffer.name = "Microphone Recording";
        }
    
        [ContextMenu("Stop Listening")]
        void StopListening()
        {
            Microphone.End(_deviceName);
        }
    }


}
