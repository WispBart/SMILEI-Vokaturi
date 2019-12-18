using System;
using System.Collections;
using System.Collections.Generic;
using SMILEI.Core;
using UnityEngine;

namespace SMILEI.Vokaturi
{
    [CreateAssetMenu(menuName = "SMILEI/Vokaturi/Mixer")]
    public class VokaturiMixerAsset : EmotionMixerAsset
    {
        [SerializeField] private VokaturiMixer _implementation = new VokaturiMixer();

        public override IEmotionMixer Implementation => _implementation;
    }
    
    [Serializable] public class VokaturiMixer : IEmotionMixer
    {
        public VokaturiEmotion Emotion;
        public VokaturiSampler Sampler;

        public VokaturiSampler.VokaturiDataEvent DataEvent;
    
        private float _lastValueReceived; 
        public float LastValueReceivedTime { get; private set; }

        private int _listenerCount = 0;

        public void Register()
        {
            _listenerCount++;
            if (_listenerCount == 1)
            {
                DataEvent = Sampler.GetDataEvent(Emotion);
                DataEvent.AddListener(OnReceive);
            }
        }

        public void Unregister()
        {
            _listenerCount--;
            if (_listenerCount == 0)
            {
                DataEvent?.RemoveListener(OnReceive);
            }
        }

        public void OnReceive(double vokaturiValue)
        {
            LastValueReceivedTime = Time.realtimeSinceStartup;
            _lastValueReceived = (float) vokaturiValue;
        }

        public Emotion GetValue()
        {
            float confidence = _lastValueReceived.Equals(0f) ? 0f : 1f;
            return new Emotion(_lastValueReceived, confidence);
        }
    }



}

