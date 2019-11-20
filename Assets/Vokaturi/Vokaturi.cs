using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Vokaturi
{
    //Todo: define the Vokaturi DLL or DLL directory

    #region HeaderFileVokaturi

    //Derived from Vokaturi.h
    //VokaturiVoice => IntPtr

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern IntPtr VokaturiVoice_create(double sample_rate, int buffer_length);

    public struct VokaturiEmotionProbabilities
    {
        public double Neutrality;
        public double Happiness;
        public double Sadness;
        public double Anger;
        public double Fear;

        public override string ToString()
        {
            double total = Neutrality + Happiness + Sadness + Anger + Fear;
            return $"Neutrality={Neutrality:0.######} Happiness={Happiness:0.######} Sadness={Sadness:0.######} Anger={Anger:0.######} Fear={Fear:0.######} Total={total}";
        }
    }

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_setRelativePriorProbabilities(IntPtr voice, VokaturiEmotionProbabilities priorEmotionProbabilities);

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fill(IntPtr voice, int num_samples, double[] samples); // deprecated; identical to VokaturiVoice_fill_float64array

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fill_float64array(IntPtr voice, int num_samples, double[] samples);

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fill_float32array(IntPtr voice, int num_samples, float[] samples);
    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fill_int32array(IntPtr voice, int num_samples, int[] samples);
    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fill_int16array(IntPtr voice, int num_samples, short[] samples);

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fill_float64value(IntPtr voice, double sample);
    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fill_float32value(IntPtr voice, float sample);
    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fill_int32value(IntPtr voice, int sample);
    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fill_int16value(IntPtr voice, int sample);   // NOT short, because of C argument sizes

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fillInterlacedStereo_float64array(IntPtr left, IntPtr right, int num_samples_per_channel, double[] samples);
    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fillInterlacedStereo_float32array(IntPtr left, IntPtr right, int num_samples_per_channel, float[] samples);
    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fillInterlacedStereo_int32array(IntPtr left, IntPtr right, int num_samples_per_channel, int[] samples);
    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_fillInterlacedStereo_int16array(IntPtr left, IntPtr right, int num_samples_per_channel, short[] samples);

    public struct VokaturiQuality
    {
        public int Valid;   // 1 = "there were voiced frames, so that the measurements are valid"; 0 = "no voiced frames found"
        public int NumFramesAnalyzed;
        public int NumFramesLost;

        public override string ToString()
        {
            return $"Valid={Valid} NumFramesAnalyzed={NumFramesAnalyzed} NumFramesLost={NumFramesLost}";
        }
    }

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_extract(IntPtr voice, ref VokaturiQuality quality, ref VokaturiEmotionProbabilities emotionProbabilities);

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_destroy(IntPtr voice);

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern void VokaturiVoice_reset(IntPtr voice);

    [DllImport(@"Assets\Vokaturi\OpenVokaturi-3-3-win64.dll")]
    public static extern IntPtr Vokaturi_versionAndLicense();

    #endregion

    public static IntPtr VoiceCreate(double sample_rate, int buffer_length)
    {
        return VokaturiVoice_create(sample_rate, buffer_length);
    }

    public static void VoiceSetRelativePriorProbabilities(IntPtr voice, VokaturiEmotionProbabilities priorEmotionProbabilities)
    {
        VokaturiVoice_setRelativePriorProbabilities(voice, priorEmotionProbabilities);
    }

    // deprecated; identical to VoiceFillFloat64Array
    public static void VoiceFill(IntPtr voice, int num_samples, double[] samples)
    {
        VokaturiVoice_fill(voice, num_samples, samples);
    }

    public static void VoiceFillFloat64Array(IntPtr voice, int num_samples, double[] samples)
    {
        VokaturiVoice_fill_float64array(voice, num_samples, samples);
    }

    public static void VoiceFillFloat32Array(IntPtr voice, int num_samples, float[] samples)
    {
        VokaturiVoice_fill_float32array(voice, num_samples, samples);
    }

    public static void VoiceFillInt32Array(IntPtr voice, int num_samples, int[] samples)
    {
        VokaturiVoice_fill_int32array(voice, num_samples, samples);
    }

    public static void VoiceFillInt16Array(IntPtr voice, int num_samples, short[] samples)
    {
        VokaturiVoice_fill_int16array(voice, num_samples, samples);
    }

    public static void VoiceFillFloat64Value(IntPtr voice, double sample)
    {
        VokaturiVoice_fill_float64value(voice, sample);
    }

    public static void VoiceFillFloat32Value(IntPtr voice, float sample)
    {
        VokaturiVoice_fill_float32value(voice, sample);
    }

    public static void VoiceFillInt32Value(IntPtr voice, int sample)
    {
        VokaturiVoice_fill_int32value(voice, sample);
    }

    public static void VoiceFillInt16Value(IntPtr voice, int sample) // NOT short, because of C argument sizes
    {
        VokaturiVoice_fill_int16value(voice, sample);
    }

    public static void VoiceFillInterlacedStereoFloat64Array(IntPtr left, IntPtr right, int num_samples_per_channel, double[] samples)
    {
        VokaturiVoice_fillInterlacedStereo_float64array(left, right, num_samples_per_channel, samples);
    }

    public static void VoiceFillInterlacedStereoFloat32Array(IntPtr left, IntPtr right, int num_samples_per_channel, float[] samples)
    {
        VokaturiVoice_fillInterlacedStereo_float32array(left, right, num_samples_per_channel, samples);
    }

    public static void VoiceFillInterlacedStereoInt32Array(IntPtr left, IntPtr right, int num_samples_per_channel, int[] samples)
    {
        VokaturiVoice_fillInterlacedStereo_int32array(left, right, num_samples_per_channel, samples);
    }

    public static void VoiceFillInterlacedStereoInt16Array(IntPtr left, IntPtr right, int num_samples_per_channel, short[] samples)
    {
        VokaturiVoice_fillInterlacedStereo_int16array(left, right, num_samples_per_channel, samples);
    }

    public static void VoiceExtract(IntPtr voice, ref VokaturiQuality quality,ref VokaturiEmotionProbabilities emotionProbabilities)
    {
        VokaturiVoice_extract(voice, ref quality, ref emotionProbabilities);
    }

    public static void VoiceDestroy(IntPtr voice)
    {
        VokaturiVoice_destroy(voice);
    }

    public static void VoiceReset(IntPtr voice)
    {
        VokaturiVoice_reset(voice);
    }

    public static string VersionAndLicense()
    {
        IntPtr ptr = Vokaturi_versionAndLicense();
        return Marshal.PtrToStringAnsi(ptr);
    }

}
