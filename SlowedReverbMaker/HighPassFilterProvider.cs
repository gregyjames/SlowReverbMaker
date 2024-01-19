using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlowedReverbMaker
{
    using NAudio.Dsp;
    using NAudio.Wave;

    public class HighPassFilterSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly BiQuadFilter lowPassFilter;

        public HighPassFilterSampleProvider(ISampleProvider source, float filterCutoff)
        {
            this.source = source;
            this.lowPassFilter = BiQuadFilter.HighPassFilter(source.WaveFormat.SampleRate, filterCutoff, 0.7071f);
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = source.Read(buffer, offset, count);

            for (int i = 0; i < samplesRead; i++)
            {
                // Apply low-pass filter to input sample
                buffer[offset + i] = lowPassFilter.Transform(buffer[offset + i]);
            }

            return samplesRead;
        }
    }

}
