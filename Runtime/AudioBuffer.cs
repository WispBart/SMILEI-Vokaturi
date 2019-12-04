using System;
using UnityEngine;

namespace SMILEI.Vokaturi
{
    //TODO: consider making part of Core
    [CreateAssetMenu(menuName = "SMILEI/Vokaturi/AudioBuffer")]
    public class AudioBuffer : ScriptableObject
    {
        public AudioClip Buffer { get; set; }

        public int WriteHeadPosition;

        public bool Ready() => Buffer != null;
    

        public void GetSamples(ref float[] samples)
        {
            if (Buffer == null)
            {
                Debug.LogError("Buffer not initialized", this);
                return;
            }
            // TODO: check length of array against array size
            Buffer.GetData(samples, WriteHeadPosition);
        }

        public void InitializeBufferIfNeeded(int sizeInSeconds, int channels, int frequency)
        {
            if (Buffer != null)
            {
                bool isSame = true;
                isSame = Buffer.length.Equals(sizeInSeconds);
                isSame &= Buffer.channels == channels;
                isSame &= Buffer.frequency == frequency;
                // delete old buffer and recreate
                Debug.Log("Initializing buffer with different specification.", this);
                if (!isSame)
                {
                    Destroy(Buffer);
                }
            }
            if (Buffer == null)
            {
                Buffer = AudioClip.Create("AudioBuffer", sizeInSeconds * channels * frequency, channels, frequency, false);
            }
        }
    
    }

}
