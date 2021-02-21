using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows an object to be flipped
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Flippable : MonoBehaviour
{
    
    private SpriteRenderer _spriteRenderer;
    
    /* *** */
    
    /// <summary>
    /// Gets the direction the object starts facing;
    /// </summary>
    public Direction1D StartingDirection = Direction1D.Right;

    /// <summary>
    /// Gets or sets the flipped state of the object
    /// </summary>
    public Direction1D Direction
    {
        get
        {
            return _spriteRenderer.flipX ? Direction1D.Left : Direction1D.Right;
        }
        set
        {
            _spriteRenderer.flipX = value == Direction1D.Left;
        }
    }

    /// <summary>
    /// Gets the normalized vector that would describe the direction an object in pointed
    /// </summary>
    /// <returns></returns>
    public Vector2 DirectionVector
    {
        get
        {
            return Direction == Direction1D.Left ? Vector2.left : Vector2.right;
        }
    }

    /// <summary>
    /// Gets the sign of the direction as it would be represented on the X axis as -1, 0 or 1
    /// </summary>
    public int DirectionSign
    {
        get => (int)Direction;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Direction = StartingDirection;
    }

    /// <summary>
    /// Flips the direction of the flippable
    /// </summary>
    public void Flip()
    {
        Direction = (Direction1D)(-(int)Direction);
    }
}
