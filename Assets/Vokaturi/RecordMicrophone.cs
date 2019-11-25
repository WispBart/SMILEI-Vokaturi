using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecordMicrophone : MonoBehaviour
{
    public bool Record = false;
    public bool WriteWav = false;
    public bool PlayWav = false;

    public string DeviceName { get; set; }
    public string[] AvailableDevices;
    public int Frequency = 44100;
    public int SamplesRecorded = 0;

    private AudioSource audioSource;

    // Start recording with built-in Microphone and play the recorded audio right away
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AvailableDevices = Microphone.devices;
    }

    void Update()
    {
        if (!Record && Microphone.IsRecording(DeviceName))
        {
            Microphone.End(DeviceName);
        }
        else if (Record && !Microphone.IsRecording(DeviceName))
        {
            audioSource.clip = Microphone.Start(DeviceName, true, 10, Frequency);
            
        }

        if (WriteWav)
        {
            float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];
            audioSource.clip.GetData(samples, 0);
            using (BinaryWriter writer = new BinaryWriter(File.Open($"recorded-samples.txt", FileMode.Create)))
            {
                foreach (float f in samples)
                {
                    writer.Write(f);
                }
            }

            WriteWav = false;
        }

        if (PlayWav && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!PlayWav && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (Microphone.IsRecording(DeviceName))
        {
            SamplesRecorded = Microphone.GetPosition(DeviceName);
        }
    }
}
