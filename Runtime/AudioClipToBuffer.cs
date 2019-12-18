using System;
using UnityEngine;

namespace SMILEI.Vokaturi
{ 
    
    public class AudioClipToBuffer : MonoBehaviour
    {
        public AudioBuffer WriteBuffer;
        public AudioClip Clip;

        public int BufferSizeInSeconds = 1;
        
        private void OnEnable()
        {
            if (Clip != null) Write();
        }
        
        [ContextMenu("Write")]
        public void Write()
        {
            var channels = Clip.channels;
            var frequency = Clip.frequency;
            WriteBuffer.Buffer = AudioClip.Create($"{BufferSizeInSeconds}SecOfRecentlyPlayedAudio", BufferSizeInSeconds * channels * frequency, channels, frequency, false);

            extraTimeSinceLastUpdate = 0f;
            TotalSamplesPlayed = 0;
            SamplesPlayedInClip = 0;
            _playing = true;
        }
        private float extraTimeSinceLastUpdate = 0;
        private int TotalSamplesPlayed = 0;
        private int SamplesPlayedInClip = 0;
        private bool _playing;
        
        void Update()
        {
            if (!_playing) return;
                
            var channels = Clip.channels;
            var frequency = Clip.frequency;
            var target = WriteBuffer.Buffer;
            
            
            extraTimeSinceLastUpdate += Time.deltaTime;
            int samplesSinceLastUpdate = (int)(channels * frequency * extraTimeSinceLastUpdate);

            //check if audioclip ended
            samplesSinceLastUpdate = Math.Min(Clip.samples - SamplesPlayedInClip, samplesSinceLastUpdate);

            extraTimeSinceLastUpdate -= (float)samplesSinceLastUpdate / (float)(channels * frequency);

            float[] samplesPlayed = new float[samplesSinceLastUpdate];
            Clip.GetData(samplesPlayed, SamplesPlayedInClip);
            SamplesPlayedInClip += samplesSinceLastUpdate;
            TotalSamplesPlayed += samplesSinceLastUpdate;
            

            target.SetData(samplesPlayed, TotalSamplesPlayed % target.samples);
            
            if (SamplesPlayedInClip == Clip.samples)
            {
                _playing = false;
                Destroy(WriteBuffer.Buffer);
            }
        }

    }
}

