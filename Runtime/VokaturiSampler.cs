using System;
using UnityEngine;
using UnityEngine.Events;

namespace SMILEI.Vokaturi
{
    public enum VokaturiEmotion
    {
        Neutrality,
        Happiness,
        Anger,
        Fear,
        Sadness
    }

    [CreateAssetMenu(menuName = "SMILEI/Vokaturi/VokaturiSampler")]
    public class VokaturiSampler : ScriptableObject, IWantsUpdate
    {
        //public delegate void VokaturiDataEvent(double value);
        public class VokaturiDataEvent : UnityEvent<double>
        {
        }

        public bool Enabled;
        public float SampleInterval;
        public int SampleSizeInSeconds;

        public AudioBuffer Buffer;

        public VokaturiEvents Events = new VokaturiEvents();

        public class VokaturiEvents
        {
            public VokaturiDataEvent Neutrality = new VokaturiDataEvent();
            public VokaturiDataEvent Happiness = new VokaturiDataEvent();
            public VokaturiDataEvent Sadness = new VokaturiDataEvent();
            public VokaturiDataEvent Anger = new VokaturiDataEvent();
            public VokaturiDataEvent Fear = new VokaturiDataEvent();
        }

        public VokaturiDataEvent GetDataEvent(VokaturiEmotion vokaturiEmotion)
        {
            switch (vokaturiEmotion)
            {
                case VokaturiEmotion.Anger:
                    return Events.Anger;
                case VokaturiEmotion.Fear:
                    return Events.Fear;
                case VokaturiEmotion.Happiness:
                    return Events.Fear;
                case VokaturiEmotion.Neutrality:
                    return Events.Neutrality;
                case VokaturiEmotion.Sadness:
                    return Events.Sadness;
                default: throw new ArgumentException("VokaturiEmotion not recognized.");
            }
        }


        [NonSerialized] private float _lastSampleTime = 0f;


        private void OnEnable()
        {
            SamplerUpdater.GetInstance().Register(this);
        }

        private void OnDisable()
        {
            SamplerUpdater.GetInstance().Unregister(this);
        }

        public void Update()
        {
            if (!Enabled) return;

            if (Time.realtimeSinceStartup - _lastSampleTime >= SampleInterval)
            {
                _lastSampleTime = Time.realtimeSinceStartup;
                if (Buffer.Ready())
                {
                    // TODO: Optimize

                    var freq = Buffer.Buffer.frequency;
                    var channels = Buffer.Buffer.channels;
                    float[] samples = new float[channels * freq * SampleSizeInSeconds];
                    Buffer.GetSamples(ref samples);
                    var results = Vokaturi.CalculateEmotion(samples, freq, channels, SampleSizeInSeconds);

                    if (results.Count > 0)
                    {
                        var result = results[0];
                        Events.Anger.Invoke(result.EmotionProbabilities.Anger);
                        Events.Fear.Invoke(result.EmotionProbabilities.Fear);
                        Events.Happiness.Invoke(result.EmotionProbabilities.Happiness);
                        Events.Neutrality.Invoke(result.EmotionProbabilities.Neutrality);
                        Events.Sadness.Invoke(result.EmotionProbabilities.Sadness);
                    }
                }
            }
        }

        public bool WantsEditorUpdate => true;
    }
}