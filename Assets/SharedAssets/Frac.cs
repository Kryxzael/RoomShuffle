using UnityEngine;

[System.Obsolete]
public struct Frac
{
    float _intern;

    public static readonly Frac MinValue = 0.0;
    public static readonly Frac MaxValue = 1.0;

    public Frac(float f)
    {
        if (f == 1)
        {
            _intern = 1;
            return;
        }

        _intern = (float)((decimal)f - (decimal)Mathf.Floor(f));
    }

    public static implicit operator float(Frac frac)
    {
        return (float)frac._intern;
    }

    public static implicit operator double(Frac frac)
    {
        return frac._intern;
    }

    public static implicit operator Frac(float f)
    {
        return new Frac(f);
    }

    public static implicit operator Frac(double d)
    {
        return new Frac((float)d);
    }

    public static Frac operator +(Frac left, Frac right)
    {
        return Mathf.Clamp01(left._intern + right._intern);
    }

    public static Frac operator -(Frac left, Frac right)
    {
        return Mathf.Clamp01(left._intern - right._intern);
    }

    public static Frac operator *(Frac left, Frac right)
    {
        return Mathf.Clamp01(left._intern * right._intern);
    }

    public static Frac operator /(Frac left, Frac right)
    {
        return Mathf.Clamp01(left._intern / right._intern);
    }

    public static Frac Min(Frac a, Frac b)
    {
        return Mathf.Min(a, b);
    }

    public static Frac Max(Frac a, Frac b)
    {
        return Mathf.Max(a, b);
    }

    public static Frac Clamp(Frac n, Frac a, Frac b)
    {
        return Mathf.Clamp(n, a, b);
    }


    public override string ToString()
    {
        return _intern.ToString();
    }

}