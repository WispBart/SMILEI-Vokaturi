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
    public class SamplerUpdater : MonoBehaviour
    {

        public List<IWantsUpdate> Samplers = new List<IWantsUpdate>();

        private static SamplerUpdater _instance;
    
        public static SamplerUpdater GetInstance()
        {
            if (_instance == null)
            {
                var go = new GameObject("Sampler Updater");

                _instance = go.AddComponent<SamplerUpdater>();
                _instance.hideFlags = HideFlags.HideAndDontSave;
                go.hideFlags = HideFlags.HideAndDontSave;
            }
            return _instance;
        }

        void OnEnable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += EditorUpdate;
#endif
        }

        void OnDisable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= EditorUpdate;
#endif
        }


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
