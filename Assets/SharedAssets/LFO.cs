using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// A syncable Low-Frequency-Oscillator
/// </summary>
[Serializable]
public class LFO : ICloneable
{
    /// <summary>
    /// The actual function of this oscillator
    /// </summary>
    public Waveshape Function;

    /// <summary>
    /// The time source used by the oscillator, By default, this is linked to system time
    /// </summary>
    public SyncingSource Source = () => DateTime.Now.Millisecond / 1000f;

    /// <summary>
    /// The speed of the LFO
    /// </summary>
    public double Frequency = 1;

    /// <summary>
    /// The offset of the phase of the LFO
    /// </summary>
    public double PhaseOffset = 0;

    /// <summary>
    /// Gets the current value of the LFO according to the LFO's syncing source
    /// </summary>
    public float Value
    {
        get
        {
            return (float)ValueFor(Source());
        }
    }

    /// <summary>
    /// Creates a new LFO instance
    /// </summary>
    /// <param name="func">Function to create LFO with</param>
    public LFO(Waveshape func)
    {
        Function = func;
    }

    /// <summary>
    /// Creates a new LFO instance
    /// </summary>
    /// <param name="func">Function to create LFO with</param>
    /// <param name="freq">Frequency of LFO</param>
    public LFO(Waveshape func, double freq)
    {
        Function = func;
        Frequency = freq;
    }

    public LFO(Waveshape func, double freq, double phase)
    {
        Function = func;
        Frequency = freq;
        PhaseOffset = phase;
    }

    /// <summary>
    /// Gets the value of the LFO for a set time
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public double ValueFor(double value)
    {
        return Math.Min(1, Math.Max(0, Function((((value + PhaseOffset) % 1) * Frequency) % 1)));
    }

    /// <summary>
    /// Emulates a reduced sample rate of the LFO
    /// </summary>
    /// <param name="samples">Sample rate</param>
    public void LoFi(int samples)
    {
        Waveshape func = Function;
        Function = x => func(Math.Round(x * samples) / (float)samples);
    }

    /// <summary>
    /// Creates a sinus wave
    /// </summary>
    /// <param name="freq">The frequency of the wave, defaults to 1</param>
    /// <param name="phase">The phase offset of the wave, defaults to 1</param>
    /// <returns></returns>
    public static LFO CreateSine(float freq = 1, float phase = 0)
    {
        return new LFO(x => (Math.Sin(x * Math.PI * 2) / 2f) + 0.5f, freq, phase);
    }

    /// <summary>
    /// Creates a sawtooth wave
    /// </summary>
    /// <param name="freq">The frequency of the wave, defaults to 1</param>
    /// <param name="phase">The phase offset of the wave, defaults to 1</param>
    /// <returns></returns>
    public static LFO CreateSaw(float freq = 1, float phase = 0)
    {
        return new LFO(x => x, freq, phase);
    }

    /// <summary>
    /// Creates a reversed sawtooth wave
    /// </summary>
    /// <param name="freq">The frequency of the wave, defaults to 1</param>
    /// <param name="phase">The phase offset of the wave, defaults to 1</param>
    /// <returns></returns>
    public static LFO CreateReversedSaw(float freq = 1, float phase = 0)
    {
        return new LFO(x => 1 - x, freq, phase);
    }

    /// <summary>
    /// Creates a triangle wave
    /// </summary>
    /// <param name="freq">The frequency of the wave, defaults to 1</param>
    /// <param name="phase">The phase offset of the wave, defaults to 1</param>
    /// <returns></returns>
    public static LFO CreateTriangle(float freq = 1, float phase = 0)
    {
        return new LFO(x => 2 * Math.Abs(x - 0.5f), freq, phase);
    }

    /// <summary>
    /// Creates a 50% pulse width pulse wave
    /// </summary>
    /// <param name="freq">The frequency of the wave, defaults to 1</param>
    /// <param name="phase">The phase offset of the wave, defaults to 1</param>
    /// <returns></returns>
    public static LFO CreateSquare(float freq = 1, float phase = 0)
    {
        return new LFO(x => Math.Round(x), freq, phase);
    }

    /// <summary>
    /// Creates a pulse wave with a custom pulse width
    /// </summary>
    /// <param name="pulseWidth">Duty Cycle of pulse</param>
    /// <param name="freq">The frequency of the wave, defaults to 1</param>
    /// <param name="phase">The phase offset of the wave, defaults to 1</param>
    /// <returns></returns>
    public static LFO CreatePulse(float pulseWidth, float freq = 1, float phase = 0)
    {
        return new LFO(x => Math.Round(x + pulseWidth - 0.5f), freq, phase);
    }

    /// <summary>
    /// Creates a square wave with a smooth transition
    /// </summary>
    /// <param name="amp">A higher number means closer to square</param>
    /// <param name="freq">The frequency of the wave, defaults to 1</param>
    /// <param name="phase">The phase offset of the wave, defaults to 1</param>
    /// <returns></returns>
    public static LFO CreateSemiSquare(float amp, float freq = 1, float phase = 0)
    {
        return new LFO(x => 2 * amp * (2 * Math.Abs(x - 0.5f)) - amp * 0.5f, freq, phase);
    }

    static Random _rng = new Random();

    /// <summary>
    /// Creates a random static-like oscillator
    /// </summary>
    /// <returns></returns>
    public static LFO CreateNoise()
    {
        return new LFO(x => _rng.NextDouble());
    }

    /// <summary>
    /// Creates a smooth sawtooth wave
    /// </summary>
    /// <param name="speed">'Smoothness' of the saw</param>
    /// <param name="freq">The frequency of the wave, defaults to 1</param>
    /// <param name="phase">The phase offset of the wave, defaults to 1</param>
    /// <returns></returns>
    public static LFO CreateSemiSaw(float speed, float freq = 1, float phase = 0)
    {
        return new LFO(x => Math.Pow(x, 1 / speed), freq, phase);
    }

    /// <summary>
    /// Creates a reversed smooth sawtooth wave
    /// </summary>
    /// <param name="speed">'Smoothness' of the saw</param>
    /// <param name="freq">The frequency of the wave, defaults to 1</param>
    /// <param name="phase">The phase offset of the wave, defaults to 1</param>
    /// <returns></returns>
    public static LFO CreateSmoothSawReversed(float speed, float freq = 1, float phase = 0)
    {
        return new LFO(x => Math.Pow(x, 1 / speed), freq, phase);
    }

    object ICloneable.Clone()
    {
        return new LFO(Function, Frequency, PhaseOffset)
        {
            Source = this.Source
        };
    }

    /// <summary>
    /// Creates an exact duplicate of this LFO
    /// </summary>
    /// <returns></returns>
    public LFO Clone()
    {
        return (this as ICloneable).Clone() as LFO;
    }

    /// <summary>
    /// Represents a waveform for oscillators
    /// </summary>
    /// <param name="x">Time</param>
    /// <returns></returns>
    public delegate double Waveshape(double x);


}

/// <summary>
/// Represents a time syncing source for LFOs and ADSRs
/// </summary>
/// <returns></returns>
public delegate double SyncingSource();