using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows an object to be flipped
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Flippable : MonoBehaviour
{
    /// <summary>
    /// Gets or sets the flipped state of the object
    /// </summary>
    public Direction1D Direction
    {
        get
        {
            return GetComponent<SpriteRenderer>().flipX ? Direction1D.LEFT : Direction1D.RIGHT;
        }
        set
        {
            GetComponent<SpriteRenderer>().flipX = value == Direction1D.LEFT;
        }
    }

    /// <summary>
    /// Gets the normalized vector that would describe the direction an object in pointed
    /// </summary>
    /// <returns></returns>
    public Vector2 GetForwardVector()
    {
        return Direction == Direction1D.LEFT ? Vector2.left : Vector2.right;
    }

    /// <summary>
    /// Flips the direction of the flippable
    /// </summary>
    public void Flip()
    {
        Direction = (Direction1D)(-(int)Direction);
    }
}
