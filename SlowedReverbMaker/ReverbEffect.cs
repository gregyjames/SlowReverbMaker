using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlowedReverbMaker
{
    public class ReverbEffect : ISampleProvider
    {
        private readonly ISampleProvider source;
        private readonly int delayBufferLength;
        private readonly float[] delayBuffer;
        private int delayBufferPosition;
        private readonly float decayFactor;
        private readonly float roomSize;
        private readonly float dampening;
        private readonly float effectStrength;

        public ReverbEffect(ISampleProvider source, float decayTimeInSeconds, float roomSize, float dampening, float effectStrength)
        {
            this.source = source;
            this.delayBufferLength = (int)(decayTimeInSeconds * source.WaveFormat.SampleRate);
            this.delayBuffer = new float[delayBufferLength];
            this.delayBufferPosition = 0;
            this.decayFactor = CalculateDecayFactor(decayTimeInSeconds);
            this.roomSize = roomSize;
            this.dampening = dampening;
            this.effectStrength = effectStrength;
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = source.Read(buffer, offset, count);

            for (int i = 0; i < samplesRead; i++)
            {
                float inputSample = buffer[offset + i];

                // Apply reverb effect using a simple feedback delay network (FDN)
                float delayedSample = delayBuffer[delayBufferPosition];
                float reverbSample = roomSize * decayFactor * delayedSample;

                // Apply dampening
                reverbSample *= (1.0f - dampening);

                // Mix the reverb effect with the original signal
                buffer[offset + i] += effectStrength * reverbSample;

                // Store the input sample in the delay buffer
                delayBuffer[delayBufferPosition] = inputSample + effectStrength * reverbSample;

                // Move to the next position in the delay buffer
                delayBufferPosition++;
                if (delayBufferPosition >= delayBufferLength)
                {
                    delayBufferPosition = 0;
                }
            }

            return samplesRead;
        }

        private float CalculateDecayFactor(float decayTimeInSeconds)
        {
            // Calculate the decay factor based on the desired decay time
            return (float)Math.Pow(10, (-3.0 / (decayTimeInSeconds * source.WaveFormat.SampleRate)));
        }
    }
}
