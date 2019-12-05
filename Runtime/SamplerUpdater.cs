using System.Collections.Generic;
using UnityEngine;


//TODO: consider making part of Core
namespace SMILEI.Vokaturi
{
    public interface IWantsUpdate
    {
        void Update();
        bool WantsEditorUpdate { get; }
    }
    
    public class SamplerUpdater
    {
        private const string UpdaterGOName = "Sampler Updater";

        public List<IWantsUpdate> Samplers = new List<IWantsUpdate>();

        private static SamplerUpdater _instance;
    
        public static SamplerUpdater GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SamplerUpdater();
                #if !UNITY_EDITOR
                StateUpdater.CreateGameObject(_instance.Update, UpdaterGOName);
                #endif
            }
            return _instance;
        }
        
        #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void RegisterPlayModeChange() => UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChange;

        static void OnPlayModeStateChange(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.EnteredPlayMode)
            {
                if (_instance != null)  StateUpdater.CreateGameObject(_instance.Update, UpdaterGOName);
            }
        }
        public SamplerUpdater()
        {
            UnityEditor.EditorApplication.update += EditorUpdate;
        }

        ~SamplerUpdater()
        {
            UnityEditor.EditorApplication.update -= EditorUpdate;
        }
        #endif



        public void Register(IWantsUpdate c)
        {
            Samplers.Add(c);
        }

        public void Unregister(IWantsUpdate c)
        {
            Samplers.Remove(c);
        }

        void Update()
        {
            foreach (var sampler in Samplers)
            {
                sampler.Update();
            }
        }

        void EditorUpdate()
        {
            if (!Application.isPlaying)
            {
                foreach (var sampler in Samplers)
                {
                    if (sampler.WantsEditorUpdate) sampler.Update();
                }
            }
        }
    }
}
