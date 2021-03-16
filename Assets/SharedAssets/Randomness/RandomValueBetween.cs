﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Represents a range of a minimum and maximum value where a random value can be picked
/// </summary>
[Serializable]
public struct RandomValueBetween
{
    [Tooltip("The minimum value of the range")]
    public float Minimum;

    [Tooltip("The maximum value of the range")]
    public float Maximum;

    /// <summary>
    /// Creates a new random value between min and max
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public RandomValueBetween(float min, float max)
    {
        Minimum = min;
        Maximum = max;
    }

    /// <summary>
    /// Gets a random number between min and max
    /// </summary>
    /// <returns></returns>
    public float Pick()
    {
        return UnityEngine.Random.Range(Minimum, Maximum);
    }

    /// <summary>
    /// Gets a random integer number between min and max
    /// </summary>
    /// <returns></returns>
    public int PickInt()
    {
        return UnityEngine.Random.Range((int)Math.Ceiling(Minimum), (int)Math.Floor(Maximum));
    }

    /// <summary>
    /// Gets a random number between min and max using the provided System.Random object
    /// </summary>
    /// <returns></returns>
    public float Pick(System.Random rng)
    {
        return Mathf.Lerp(Minimum, Maximum, (float)rng.NextDouble());
    }

    /// <summary>
    /// Gets a random number integer between min and max using the provided System.Random object
    /// </summary>
    /// <returns></returns>
    public int PickInt(System.Random rng)
    {
        return rng.Next((int)Math.Ceiling(Minimum), (int)Math.Floor(Maximum));
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Minimum + ".." + Maximum;
    }
}
