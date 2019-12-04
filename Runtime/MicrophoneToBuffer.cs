using UnityEngine;

namespace SMILEI.Vokaturi
{
    public class MicrophoneToBuffer : MonoBehaviour
    {
        public AudioBuffer WriteBuffer;
        public string DeviceName;
        public int Frequency;
        public int SizeInSeconds;
    
        private string _deviceName => string.IsNullOrEmpty(DeviceName) ? null : DeviceName;
        private void OnEnable() => StartListening();
        private void OnDisable() => StopListening();
    

        [ContextMenu("Start Listening")]
        void StartListening()
        {
            if (WriteBuffer == null) return;
            WriteBuffer.InitializeBufferIfNeeded(SizeInSeconds, 1, Frequency);
            WriteBuffer.Buffer = Microphone.Start(_deviceName, true, SizeInSeconds, Frequency);
        }
    
        [ContextMenu("Stop Listening")]
        void StopListening()
        {
            Microphone.End(_deviceName);
        }
    }


}
