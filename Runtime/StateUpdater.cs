using UnityEngine;

namespace SMILEI.Vokaturi
{
    public class StateUpdater : MonoBehaviour
    {
        public delegate void Callback();

        public static void CreateGameObject(Callback callback, string name)
        {
            var go = new GameObject(name);

            DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.DontSave;

            var updater = go.AddComponent<StateUpdater>();
            updater._callback = callback;
        }

        Callback _callback;

        void Update()
        {
            _callback();
        }
    }

}

