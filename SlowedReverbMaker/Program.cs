using System;
using NAudio.Dmo.Effect;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

using NAudio.Wave;
using System;
using SlowedReverbMaker;

class Program
{
    static void Main()
    {
        // Example: Slow down audio, apply reverb, and save to a new file
        SlowDownAndApplyReverb("your_input_audio.mp3", "your_output_slowed_and_reverberated_audio.wav", slowdownFactor: .750f);
    }

    static void SlowDownAndApplyReverb(string inputFilePath, string outputFilePath, float slowdownFactor)
    {
        using (var reader = new MediaFoundationReader(inputFilePath))
        {
            var sampleProvider = reader.ToSampleProvider();

            var slowed = new SmbPitchShiftingSampleProvider(sampleProvider, 1024, 2, slowdownFactor);
            var echo = new EchoSampleProvider(slowed, 825, 0.35f, 0.25f);
            var reverbEffect = new ReverbEffect(echo, .5f, 5f, 0.825f, 0.25f);
            var filter = new HighPassFilterSampleProvider(reverbEffect, 25);
            var boosted_vocals = new BoostVocalSampleProvider(filter, 1.5f, 127.5f);
            WaveFileWriter.CreateWaveFile16(outputFilePath, boosted_vocals);
        }
    }
}
