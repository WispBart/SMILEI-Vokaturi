using System;
using System.Collections.Generic;
using UnityEngine;

namespace SMILEI.Vokaturi
{
    public struct VokaturiAnalysis
{
    public VokaturiLib.VokaturiQuality Quality;
    public VokaturiLib.VokaturiEmotionProbabilities EmotionProbabilities;

    public VokaturiAnalysis(VokaturiLib.VokaturiQuality quality, VokaturiLib.VokaturiEmotionProbabilities emotionProbabilities)
    {
        Quality = quality;
        EmotionProbabilities = emotionProbabilities;
    }
}

public static class Vokaturi
{
    private static IntPtr[] voices;

    //calculates emotion for the whole audioClip
    public static List<VokaturiAnalysis> CalculateEmotion(AudioClip audioClip)
    {
        return CalculateEmotion(audioClip, 0, audioClip.length);
    }

    //calculates emotion for the whole audioClip, starting at offsetSeconds and wraps automatically
    public static List<VokaturiAnalysis> CalculateEmotion(AudioClip audioClip, float offsetSeconds)
    {
        return CalculateEmotion(audioClip, offsetSeconds, audioClip.length);
    }

    //calculates emotion for the audioClip, starting at offsetSeconds for length of duration and when needed wraps automatically
    public static List<VokaturiAnalysis> CalculateEmotion(AudioClip audioClip, float offsetSeconds, float duration)
    {
        if (duration < 1) throw new ArgumentException("Audioclip requires minimum length of 1.0s!");
        if (audioClip.channels > 2) throw new ArgumentException("Audioclip more than 2 channels not supported!");

        int numSamplesPerChannel = (int)(duration * audioClip.frequency);
        int numSamples = numSamplesPerChannel * audioClip.channels;

        if (numSamples > audioClip.samples) throw new ArgumentException($"Audioclip is smaller ({audioClip.samples}) than requested number of samples ({numSamples})!");

        //create voices for each channel
        voices = new IntPtr[audioClip.channels];
        for (int i = 0; i < audioClip.channels; i++)
        {
            voices[i] = VokaturiLib.VoiceCreate(audioClip.frequency, numSamplesPerChannel);
        }

        int offsetSamples = (int)Math.Round(offsetSeconds * audioClip.frequency * audioClip.channels);

        float[] samples = new float[numSamples];
        audioClip.GetData(samples, offsetSamples); //automatically wraps when: offsetSamples + samples.Length > audioClip.samples

        if (audioClip.channels == 2)
        {
            VokaturiLib.VoiceFillInterlacedStereoFloat32Array(voices[0], voices[1], samples.Length / 2, samples);
        }
        else
            VokaturiLib.VoiceFillFloat32Array(voices[0], samples.Length, samples);

        VokaturiLib.VokaturiQuality quality = new VokaturiLib.VokaturiQuality();
        VokaturiLib.VokaturiEmotionProbabilities emotionProbabilities = new VokaturiLib.VokaturiEmotionProbabilities();

        List<VokaturiAnalysis> vokaturiAnalyses = new List<VokaturiAnalysis>();

        for (int i = 0; i < audioClip.channels; i++)
        {
            VokaturiLib.VoiceExtract(voices[i], ref quality, ref emotionProbabilities);
            vokaturiAnalyses.Add(new VokaturiAnalysis(quality,emotionProbabilities));

            //Debug.Log($"Channel {i}: {quality}");
            //Debug.Log($"Channel {i}: {emotionProbabilities}");
        }

        //destroy voices
        for (int i = 0; i < audioClip.channels; i++)
        {
            VokaturiLib.VoiceDestroy(voices[i]);
        }

        return vokaturiAnalyses;
    }
    
        public static List<VokaturiAnalysis> CalculateEmotion(float[] samples, int frequency, int channels, float duration)
    {
        if (duration < 1) throw new ArgumentException("Audioclip requires minimum length of 1.0s!");

        int numSamplesPerChannel = (int)(duration * frequency);
        
        //create voices for each channel
        voices = new IntPtr[channels];
        for (int i = 0; i < channels; i++)
        {
            voices[i] = VokaturiLib.VoiceCreate(frequency, numSamplesPerChannel);
        }

        if (channels == 2)
        {
            VokaturiLib.VoiceFillInterlacedStereoFloat32Array(voices[0], voices[1], samples.Length / 2, samples);
        }
        else
            VokaturiLib.VoiceFillFloat32Array(voices[0], samples.Length, samples);

        VokaturiLib.VokaturiQuality quality = new VokaturiLib.VokaturiQuality();
        VokaturiLib.VokaturiEmotionProbabilities emotionProbabilities = new VokaturiLib.VokaturiEmotionProbabilities();

        List<VokaturiAnalysis> vokaturiAnalyses = new List<VokaturiAnalysis>();

        for (int i = 0; i < channels; i++)
        {
            VokaturiLib.VoiceExtract(voices[i], ref quality, ref emotionProbabilities);
            vokaturiAnalyses.Add(new VokaturiAnalysis(quality,emotionProbabilities));

            //Debug.Log($"Channel {i}: {quality}");
            //Debug.Log($"Channel {i}: {emotionProbabilities}");
        }

        //destroy voices
        for (int i = 0; i < channels; i++)
        {
            VokaturiLib.VoiceDestroy(voices[i]);
        }

        return vokaturiAnalyses;
    }
}


}
