using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VokaturiTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        AudioClip audioClip = audioSource.clip;

        Vokaturi.CalculateEmotion(audioClip);

        Vokaturi.CalculateEmotion(audioClip, 0,1);
        Vokaturi.CalculateEmotion(audioClip, 3.99F,1);

        for (int i = 0; i < audioClip.length; i++) 
        {
          //  Vokaturi.CalculateEmotion(audioClip, i, 1);
        }

        MicrophoneRecorder microphoneRecorder;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
