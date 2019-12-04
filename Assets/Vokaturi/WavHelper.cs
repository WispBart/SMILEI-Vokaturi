using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class WavHelper
{
    public static void WriteWavData(string filename, AudioClip audioClip, int offset)
    {
        WriteWavData(filename, audioClip, offset, audioClip.samples);
    }

    public static void WriteWavData(string filename, AudioClip audioClip, int sampleOffset, int numSamples)
    {
        //if (sampleOffset + numSamples > audioClip.samples) throw new ArgumentException("too long");

        float[] allSamples = new float[numSamples];
        audioClip.GetData(allSamples, sampleOffset);

        using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
        {
            foreach (float f in allSamples)
            {
                writer.Write(f);
            }
        }
    }
}
