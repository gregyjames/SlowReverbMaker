using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlowedReverbMaker
{
    public class EchoSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly int echoDelayInSamples;
        private readonly float echoGain;
        private readonly float decay;
        private readonly float[] delayBuffer;

        private int position;

        public EchoSampleProvider(ISampleProvider source, int echoDelayInMilliseconds, float echoGain, float decay)
        {
            this.source = source;
            this.echoDelayInSamples = (int)(source.WaveFormat.SampleRate * echoDelayInMilliseconds / 1000.0);
            this.echoGain = echoGain;
            this.decay = decay;
            this.delayBuffer = new float[echoDelayInSamples];
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = source.Read(buffer, offset, count);

            for (int i = 0; i < samplesRead; i++)
            {
                float inputSample = buffer[offset + i];

                // Apply echo effect with decay
                float echoSample = delayBuffer[position];
                delayBuffer[position] = inputSample + echoSample * echoGain;
                buffer[offset + i] += echoSample;

                // Apply decay to echoed sample
                delayBuffer[position] *= decay;

                position++;
                if (position >= echoDelayInSamples)
                {
                    position = 0;
                }
            }

            return samplesRead;
        }
    }
}
