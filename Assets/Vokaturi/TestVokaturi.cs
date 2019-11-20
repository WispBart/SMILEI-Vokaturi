using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

//Todo: alleen mono werkt?!

public class TestVokaturi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Vokaturi.VersionAndLicense());

        AudioSource audioSource = GetComponent<AudioSource>();

        float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];
        audioSource.clip.GetData(samples, 0);

        Debug.Log($"samples.Length={samples.Length} audioSource.clip.channels={audioSource.clip.channels}");

        float duration = 1.0f; //minimum duration is 1.0s
        int numSamplesPerChannel = (int)(duration * audioSource.clip.frequency);
        int numSamples = numSamplesPerChannel * audioSource.clip.channels;

        IntPtr voiceFirstChannel = Vokaturi.VoiceCreate(audioSource.clip.frequency, numSamplesPerChannel);
        IntPtr voiceSecondChannel = new IntPtr();
        if (audioSource.clip.channels == 2)
        {
            voiceSecondChannel = Vokaturi.VoiceCreate(audioSource.clip.frequency, numSamplesPerChannel);
        }

        int numSteps = (int)Math.Floor(audioSource.clip.length / (float)duration);
        if (audioSource.clip.length > (numSteps * (float)duration)) numSteps++;

        float startOffsetSeconds = 0;
        for (int i = 0; i < numSteps; i++)
        {
            float endOffsetSeconds = startOffsetSeconds + duration;
            endOffsetSeconds = Math.Min(endOffsetSeconds, audioSource.clip.length);

            float[] samplesPart = samples
                .Skip((int)(startOffsetSeconds * numSamples))
                .ToArray();

            if (samplesPart.Length > numSamples) samplesPart = samplesPart.Take(numSamples).ToArray();

            using (BinaryWriter writer = new BinaryWriter(File.Open($"float{i}.txt", FileMode.Create)))
            {
                foreach (float f in samplesPart)
                {
                    writer.Write(f);
                }
            }

            if (audioSource.clip.channels == 2)
            {
                Vokaturi.VoiceFillInterlacedStereoFloat32Array(voiceFirstChannel, voiceSecondChannel,
                    samplesPart.Length / 2, samplesPart);
            }
            else
                Vokaturi.VoiceFillFloat32Array(voiceFirstChannel, samplesPart.Length, samplesPart);

            Vokaturi.VokaturiQuality quality = new Vokaturi.VokaturiQuality();
            Vokaturi.VokaturiEmotionProbabilities emotionProbabilities = new Vokaturi.VokaturiEmotionProbabilities();

            Debug.Log($"From {startOffsetSeconds}-{endOffsetSeconds}");
            Vokaturi.VoiceExtract(voiceFirstChannel, ref quality, ref emotionProbabilities);
            Debug.Log($"First Channel {quality}");
            Debug.Log($"First Channel {emotionProbabilities}");

            if (audioSource.clip.channels == 2)
            {
                Vokaturi.VoiceExtract(voiceSecondChannel, ref quality, ref emotionProbabilities);
                Debug.Log($"Second Channel {quality}");
                Debug.Log($"Second Channel {emotionProbabilities}");
            }

            startOffsetSeconds += duration;
        }

        Vokaturi.VoiceDestroy(voiceFirstChannel);
        if (audioSource.clip.channels == 2)
        {
            Vokaturi.VoiceDestroy(voiceSecondChannel);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
