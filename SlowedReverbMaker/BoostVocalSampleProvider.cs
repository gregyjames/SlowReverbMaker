using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Dsp;
using NAudio.Wave;
using NAudio.Wave;

namespace SlowedReverbMaker
{

    public class BoostVocalSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly float boostFactor;
        private readonly BiQuadFilter[] filters;

        public BoostVocalSampleProvider(ISampleProvider source, float boostFactor, float centerFrequency)
        {
            this.source = source;
            this.boostFactor = boostFactor;

            // Create a series of peaking filters to boost vocal frequencies
            this.filters = new BiQuadFilter[]
            {
            BiQuadFilter.PeakingEQ(source.WaveFormat.SampleRate, centerFrequency, 2.0f, boostFactor),
                // Add more filters if needed
            };
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = source.Read(buffer, offset, count);

            // Apply equalization to boost vocal frequencies
            for (int i = 0; i < samplesRead; i++)
            {
                foreach (var filter in filters)
                {
                    buffer[offset + i] = filter.Transform(buffer[offset + i]);
                }
            }

            return samplesRead;
        }
    }



}
