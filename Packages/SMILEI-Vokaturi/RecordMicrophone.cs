using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecordMicrophone : MonoBehaviour
{
    public bool Record = false;
    public bool PlayWav = false;

    [Tooltip("RecordingBufferSize in seconds")]
    public int RecordingBufferSize = 10;
    public int RecordingDeviceIndex = 0;
    public string DeviceName
    {
        get { return AvailableRecordingDevices[RecordingDeviceIndex]; }
    }
    public string[] AvailableRecordingDevices;
    public int Frequency = 44100;

    public int SamplesRecorded {
        get
        {
            if (Microphone.IsRecording(DeviceName))
            {
                return Microphone.GetPosition(DeviceName);
            }
            else return 0;
        }
    }

    public float TimeRecorded
    {
        get
        {
            return (float) SamplesRecorded / (float) Frequency;
        }
    }

    private AudioSource audioSource;

    // Start recording with built-in Microphone and play the recorded audio right away
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AvailableRecordingDevices = Microphone.devices;
    }

    //public float[] GetSamples(float )
    //{
    //    float[] samples = new float[RecordingBufferSize * audioSource.clip.frequency * audioSource.clip.channels];
    //    audioSource.clip.GetData(samples,)
    //    return samples;
    //}

    void Update()
    {
        if (!Record && Microphone.IsRecording(DeviceName))
        {
            Microphone.End(DeviceName);
        }
        else if (Record && !Microphone.IsRecording(DeviceName))
        {
            audioSource.clip = Microphone.Start(DeviceName, true, RecordingBufferSize, Frequency);
            
        }

        if (PlayWav && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!PlayWav && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        
    }
}
