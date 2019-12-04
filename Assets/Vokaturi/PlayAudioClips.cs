using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public struct AudioData
{
    public int Nr;
    public float[] Data;

    public static int LastNr = 0;

    public AudioData(float[] data)
    {
        Nr = LastNr;
        Data = data;
        LastNr++;
    }
}

public class PlayAudioClips : MonoBehaviour
{
    public AudioClip CurrentAudioClip => AudioClipsToPlay[AudioClipIndex];
    public List<AudioClip> AudioClipsToPlay;
    public int AudioClipIndex = 0;

    public int RecentlyPlayedAudioSizeInSeconds = 1;
    public AudioClip RecentlyPlayedAudio;

    public bool Play = false;
    public bool PlayClip = false;

    private AudioSource audioSource;

    private float extraTimeSinceLastUpdate = 0;
    private int TotalSamplesPlayed = 0;
    private int SamplesPlayedInClip = 0;

    public float VokaturiInterval = 0.5F;
    private float VokaturiIntervalRemaining;

    private int Frequency;
    private int Channels;

    [Range(0, 1)]
    public double Neutrality;
    [Range(0, 1)]
    public double Happiness;
    [Range(0, 1)]
    public double Sadness;
    [Range(0, 1)]
    public double Anger;
    [Range(0, 1)]
    public double Fear;

    // Start is called before the first frame update
    void Start()
    {

        CheckAudioClips();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = RecentlyPlayedAudio;

        VokaturiIntervalRemaining = VokaturiInterval;
    }

    void CheckAudioClips()
    {
        Channels = AudioClipsToPlay[0].channels;
        Frequency = AudioClipsToPlay[0].frequency;

        //1 sec clip
        RecentlyPlayedAudio = AudioClip.Create($"{RecentlyPlayedAudioSizeInSeconds}SecOfRecentlyPlayedAudio", RecentlyPlayedAudioSizeInSeconds * Channels * Frequency, Channels, Frequency, false);

        //checks clips
        for (int i = 0; i < AudioClipsToPlay.Count; i++)
        {
            if (AudioClipsToPlay[i].channels != Channels || AudioClipsToPlay[i].frequency != Frequency)
                throw new ArgumentException($"Some audioclips don't have {Channels} channels and/or have {Frequency} frequency!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayClip && !audioSource.isPlaying)
        {
            audioSource.Play();
            PlayClip = false;

        }
        else if (!PlayClip && audioSource.isPlaying)
        {
            //audioSource.Stop();
        }

        if (Play)
        {
            extraTimeSinceLastUpdate += Time.deltaTime;
            int samplesSinceLastUpdate = (int)(Channels * Frequency * extraTimeSinceLastUpdate);

            //check if audioclip ended
            samplesSinceLastUpdate = Math.Min(CurrentAudioClip.samples - SamplesPlayedInClip, samplesSinceLastUpdate);

            extraTimeSinceLastUpdate -= (float)samplesSinceLastUpdate / (float)(Channels * Frequency);

            float[] samplesPlayed = new float[samplesSinceLastUpdate];
            CurrentAudioClip.GetData(samplesPlayed, SamplesPlayedInClip);
            SamplesPlayedInClip += samplesSinceLastUpdate;
            TotalSamplesPlayed += samplesSinceLastUpdate;

            RecentlyPlayedAudio.SetData(samplesPlayed, TotalSamplesPlayed % RecentlyPlayedAudio.samples);

            if (SamplesPlayedInClip == CurrentAudioClip.samples)
            {
                WavHelper.WriteWavData( "CurrentAudioClip.txt", CurrentAudioClip,0);

                //TODO: check channels for each clip
                AudioClipIndex = (AudioClipIndex + 1) % AudioClipsToPlay.Count; //next and wrap
                Debug.Log($"Switching to next audiofile {CurrentAudioClip.name}");
                SamplesPlayedInClip = 0;
                Play = false;

                WavHelper.WriteWavData( "RecentlyPlayedAudio.txt", RecentlyPlayedAudio,0);
            }

            VokaturiIntervalRemaining -= Time.deltaTime;

            if (VokaturiIntervalRemaining <= 0)
            {
                //calculate emotions
                List<VokaturiAnalysis> vokaturiAnalyses = Vokaturi.CalculateEmotion(RecentlyPlayedAudio);
                Neutrality = vokaturiAnalyses.Average(va => va.EmotionProbabilities.Neutrality);
                Happiness = vokaturiAnalyses.Average(va => va.EmotionProbabilities.Happiness);
                Sadness = vokaturiAnalyses.Average(va => va.EmotionProbabilities.Sadness);
                Anger = vokaturiAnalyses.Average(va => va.EmotionProbabilities.Anger);
                Fear = vokaturiAnalyses.Average(va => va.EmotionProbabilities.Fear);

                VokaturiIntervalRemaining = VokaturiInterval;
            }
        }
    }
}

