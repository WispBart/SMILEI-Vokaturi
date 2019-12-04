using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneRecorder : MonoBehaviour
{
    public bool Record = false;

    [Tooltip("RecordingBufferSize in seconds")]
    public int RecordingBufferSize = 2;
    public int RecordingDeviceIndex = 0;
    public string DeviceName
    {
        get
        {
            if (RecordingDeviceIndex >= AvailableRecordingDevices.Length) throw new ArgumentException("Recording device index does not exist!");
            return AvailableRecordingDevices[RecordingDeviceIndex];
        }
    }
    public string[] AvailableRecordingDevices;
    public int Frequency = 44100;

    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        AvailableRecordingDevices = Microphone.devices;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Record && Microphone.IsRecording(DeviceName))
        {
            Microphone.End(DeviceName);
        }
        else if (Record && !Microphone.IsRecording(DeviceName))
        {
            audioClip = Microphone.Start(DeviceName, true, RecordingBufferSize, Frequency);
        }

    }
}
