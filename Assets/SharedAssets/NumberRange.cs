using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct NumberRange
{
    public static readonly NumberRange Zero = new NumberRange(0, 0);
    public static readonly NumberRange Infinite = new NumberRange(float.NegativeInfinity, float.PositiveInfinity);

    [SerializeField]
    float min, max;

    public float Start
    {
        get
        {
            return min;
        }
        set
        {
            if (value > End)
            {
                throw new ArgumentException("Start cannot be greater than End");
            }

            min = value;
        }
    }

    public float End
    {
        get
        {
            return max;
        }
        set
        {
            if (value < Start)
            {
                throw new ArgumentException("Start cannot be greater than End");
            }

            max = value;
        }
    }

    public bool HasStart
    {
        get
        {
            return !float.IsInfinity(Start);
        }
    }

    public bool HasEnd
    {
        get
        {
            return !float.IsInfinity(End);
        }
    }

    public bool IsInfinite
    {
        get
        {
            return !HasStart || !HasEnd;
        }
    }

    public bool IsSingle
    {
        get
        {
            return Start == End;
        }
    }

    public float Length
    {
        get
        {
            return End - Start + 1;
        }
    }

    public NumberRange(float start, float end)
    {
        if (start > end)
        {
            throw new ArgumentException("Start cannot be greater than End");
        }

        min = start;
        max = end;
    }

    public bool InRange(float a)
    {
        return a >= Start && a <= End;
    }

    public void Assert(float a)
    {
        if (!InRange(a))
        {
            throw new Exception("Assertion failed on " + ToString().ToLower() + " with value " + a);
        }
    }

    public int RandomInt()
    {
        return Mathf.CeilToInt(UnityEngine.Random.Range(Mathf.Ceil(Start), Mathf.Floor(End)));
    }

    public float RandomFloat()
    {
        return UnityEngine.Random.Range(Start, End);
    }

    public float Clamp(float a)
    {
        return Mathf.Clamp(a, Start, End);
    }

    public int Clamp(int a)
    {
        return Mathf.Clamp(a, Mathf.CeilToInt(Start), Mathf.FloorToInt(End));
    }

    public IEnumerable<int> ToIntArray()
    {
        if (IsInfinite)
        {
            throw new InvalidOperationException("Cannot enumerate infinite range");
        }

        return Enumerable.Range(Mathf.CeilToInt(Start), Mathf.FloorToInt(Length));
    }

    public override string ToString()
    {
        return "Range: [" + Start + "-" + End + "]";
    }

    public override bool Equals(object obj)
    {
        if (obj is NumberRange)
        {
            return this == (NumberRange)obj;
        }

        return false;
    }

    public static bool operator ==(NumberRange left, NumberRange right)
    {
        return left.Start == right.Start && left.End == right.End;
    }

    public static bool operator !=(NumberRange left, NumberRange right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return Start.GetHashCode() ^ End.GetHashCode();
    }


}
