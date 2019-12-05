using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMILEI.Vokaturi
{ 
    
    public class AudioClipToBuffer : MonoBehaviour
    {
        public AudioBuffer WriteBuffer;
        public AudioClip Clip;

        private void OnEnable()
        {
            if (Clip != null) Write();
        }
        
        [ContextMenu("Write")]
        public void Write()
        {
            WriteBuffer.Buffer = Clip;
        }

    }
}

