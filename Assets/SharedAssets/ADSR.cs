using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// A general purpose Attack-Decay-Sustain-Release envelope controller
/// </summary>
[Serializable]
public class ADSR : ICloneable
{
    public static readonly ADSR Default = new ADSR(0, 0, 1, 0);

    /// <summary>
    /// Time for initial fadein of envelope
    /// </summary>
    public int AttackTime;
    /// <summary>
    /// Time for fadeout to sustain level of envelope
    /// </summary>
    public int DecayTime;

    /// <summary>
    /// The final level of the envelope before release
    /// </summary>
    public float SustainLevel;

    /// <summary>
    /// Time for final fadeout of envelope
    /// </summary>
    public int ReleaseTime;

    /// <summary>
    /// The time source used by the envelope, By default, this is linked to system time
    /// </summary>
    public SyncingSource Source = () => DateTime.Now.Ticks / 1E4;

    /// <summary>
    /// Creates a new envelope
    /// </summary>
    /// <param name="a">Attack time</param>
    /// <param name="d">Decay time</param>
    /// <param name="s">Sustain level</param>
    /// <param name="r">Release time</param>
    public ADSR(int a, int d, float s, int r)
    {
        AttackTime = a;
        DecayTime = d;
        SustainLevel = s;
        ReleaseTime = r;
    }

    /// <summary>
    /// Gets the current value of the ADSR envelope according to the syncing source
    /// </summary>
    public float Value
    {
        get
        {
            double ticks = _ticksSinceLastSwitch;

            switch (_state)
            {
                case State.Awaiting:
                    return 0;
                case State.Attacking:
                    //If we are bellow the attack time
                    if (ticks < AttackTime)
                    {
                        return (float)ticks / AttackTime;
                    }
                    //If we are above attack time (decaying);
                    else
                    {
                        ticks -= AttackTime;
                        //Are we decaying?
                        if (ticks < DecayTime)
                        {
                            return (1f - ((float)ticks / DecayTime) * (1 - SustainLevel));
                        }
                        return SustainLevel;
                    }
                case State.Releasing:
                    //Are we done releasing?
                    if (ticks > ReleaseTime)
                    {
                        return 0;
                    }

                    return (1f - ((float)ticks / ReleaseTime)) * _lastValueBeforeRelease;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// Has this envelope been triggered, awaiting release
    /// </summary>
    public bool Attacking
    {
        get
        {
            return _state == State.Attacking;
        }
    }

    /// <summary>
    /// Private: The time of the last state switch
    /// </summary>
    private double _timeOfLastSwitch;

    /// <summary>
    /// Private: The last value of the envelope before release for triggered
    /// </summary>
    private float _lastValueBeforeRelease { get; set; }

    /// <summary>
    /// Private: Internal state of envelope
    /// </summary>
    private State _state;

    /// <summary>
    /// Private: The amount of ticks since last state switch
    /// </summary>
    private double _ticksSinceLastSwitch
    {
        get
        {
            return Source() - _timeOfLastSwitch;
        }
    }

    /// <summary>
    /// Starts the envelope
    /// </summary>
    public void Attack()
    {
        _timeOfLastSwitch = Source();
        _state = State.Attacking;
    }

    /// <summary>
    /// Stops the envelope
    /// </summary>
    public void Release()
    {
        _lastValueBeforeRelease = Value;
        _timeOfLastSwitch = Source();
        _state = State.Releasing;
    }

    object ICloneable.Clone()
    {
        return new ADSR(AttackTime, DecayTime, SustainLevel, ReleaseTime)
        {
            _state = _state,
            _timeOfLastSwitch = _timeOfLastSwitch,
            _lastValueBeforeRelease = _lastValueBeforeRelease,
            Source = Source
        };
    }

    /// <summary>
    /// Creates an exact duplicate of this ADSR with internal state intact
    /// </summary>
    /// <returns></returns>
    public ADSR Clone()
    {
        return (this as ICloneable).Clone() as ADSR;
    }

    private enum State
    {
        Awaiting,
        Attacking,
        Releasing
    }
}