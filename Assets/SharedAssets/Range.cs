using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Range<T> where T : IComparable<T>
{
    [SerializeField]
    public T Minimum = default(T);

    [SerializeField]
    public T Maximum = default(T);

    public bool IsValid
    {
        get
        {
            return Minimum.CompareTo(Maximum) != 1;
        }
    }

    public bool IsSingle
    {
        get
        {
            return Minimum.CompareTo(Maximum) == 0;
        }
    }

    public bool HasStart
    {
        get
        {
            if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
            {
                return !float.IsInfinity((float)Convert.ChangeType(Minimum, typeof(float)));
            }

            return Minimum != null;
        }
    }

    public bool HasEnd
    {
        get
        {
            if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
            {
                return !float.IsInfinity((float)Convert.ChangeType(Maximum, typeof(float)));
            }

            return Maximum != null;
        }
    }

    public bool IsInfinite
    {
        get
        {
            return !HasEnd || !HasStart;
        }
    }

    [SerializeField]
    public bool Inclusive;

    public Range()
    {

    }

    public Range(T minimum, T maximum, bool inclusive)
    {
        Minimum = minimum;
        Maximum = maximum;
        Inclusive = inclusive;
    }

    public Range(T minimum, T maximum) : this(minimum, maximum, true)
    {

    }

    public bool InRange(T value)
    {
        if (Inclusive)
        {
            return value.CompareTo(Minimum) != -1 && value.CompareTo(Maximum) != 1;
        }
        return value.CompareTo(Minimum) == 1 && value.CompareTo(Maximum) == -1;
    }

    public override string ToString()
    {
        return "Range: [" + Minimum.ToString() + "->" + Maximum.ToString() + "]";
    }
}