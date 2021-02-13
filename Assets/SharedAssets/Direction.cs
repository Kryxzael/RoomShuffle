using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents one of four directions
/// </summary>
public enum Direction4
{
    Up = 0,
    Right = 2,
    Down = 4,
    Left = 6,
}

/// <summary>
/// Represents one of eight directions
/// </summary>
public enum Direction8
{
    Up = 0,
    UpRight = 1,
    Right = 2,
    DownRight = 3,
    Down = 4,
    DownLeft = 5,
    Left = 6,
    UpLeft = 7
}

public static class DirectionExtensions
{
    /// <summary>
    /// Converts this Direction8 into a Direction4
    /// </summary>
    /// <param name="dir8"></param>
    /// <returns></returns>
    public static Direction4 ToDirection4(this Direction8 dir8)
    {
        //Argument is not is a compound direction.
        if ((int)dir8 % 2 == 1)
        {
            throw new ArgumentException("Cannot convert direction " + dir8 + " to cardinal");
        }

        return (Direction4)dir8;
    }

    /// <summary>
    /// Converts the Direction4 into a Direction8
    /// </summary>
    /// <param name="dir4"></param>
    /// <returns></returns>
    public static Direction8 ToDirection8(this Direction4 dir4)
    {
        return (Direction8)dir4;
    }

    /// <summary>
    /// Rotates the direction a set amount of times
    /// </summary>
    /// <param name="dir4"></param>
    public static Direction4 Rotate(this Direction4 dir4, int times = 1)
    {
        if (times >= 0)
        {
            return (Direction4)(((int)dir4 + times * 2) % 8);
        }

        throw new ArgumentException("Argument must be greater than or equal to zero");
    }

    /// <summary>
    /// Rotates the direction a set amount of times
    /// </summary>
    /// <param name="dir8"></param>
    public static Direction8 Rotate(this Direction8 dir8, int times)
    {
        if (times >= 0)
        {
            return (Direction8)(((int)dir8 + times) % 8);
        }

        throw new ArgumentException("Argument must be greater than or equal to zero");
    }

    /// <summary>
    /// Flips the direction 180 degrees
    /// </summary>
    /// <param name="dir8"></param>
    /// <returns></returns>
    public static Direction4 Flip(this Direction4 dir4)
    {
        return dir4.Rotate(2);
    }

    /// <summary>
    /// Flips the direction 180 degrees
    /// </summary>
    /// <param name="dir8"></param>
    /// <returns></returns>
    public static Direction8 Flip(this Direction8 dir8)
    {
        return dir8.Rotate(4);
    }

    /// <summary>
    /// Constructs a Direction8 based on two sign values (0, 1 or -1)
    /// </summary>
    /// <param name="signX"></param>
    /// <param name="signY"></param>
    /// <returns></returns>
    public static Direction8 Construct(float signX, float signY)
    {
        signX = Math.Sign(signX);
        signY = Math.Sign(signY);

        if (signX == -1 && signY == -1) return Direction8.DownLeft;
        if (signX == -1 && signY == +0) return Direction8.Left;
        if (signX == -1 && signY == +1) return Direction8.UpLeft;

        if (signX == +0 && signY == +1) return Direction8.Up;
        if (signX == +0 && signY == +0) throw new ArgumentOutOfRangeException("Cannot construct empty Direction8");
        if (signX == +0 && signY == -1) return Direction8.Down;

        if (signX == +1 && signY == -1) return Direction8.DownRight;
        if (signX == +1 && signY == +0) return Direction8.Right;
        if (signX == +1 && signY == +1) return Direction8.UpRight;

        throw new ArgumentOutOfRangeException("Invalid arguments");
    }

    /// <summary>
    /// Converts the direction into a Vector2 instance
    /// </summary>
    /// <param name="dir8"></param>
    /// <returns></returns>
    public static Vector2 ToVector2(this Direction8 dir8)
    {
        switch (dir8)
        {
            case Direction8.Up:
                return Vector2.up;
            case Direction8.UpRight:
                return new Vector2(1, 1).normalized;
            case Direction8.Right:
                return Vector2.right;
            case Direction8.DownRight:
                return new Vector2(1, -1).normalized;
            case Direction8.Down:
                return Vector2.down;
            case Direction8.DownLeft:
                return new Vector2(-1, -1).normalized;
            case Direction8.Left:
                return Vector2.left;
            case Direction8.UpLeft:
                return new Vector2(-1, 1).normalized;
            default:
                throw new ArgumentOutOfRangeException("Not a valid direction");
        }
    }

    /// <summary>
    /// Converts the direction into a Vector2 instance
    /// </summary>
    /// <param name="dir8"></param>
    /// <returns></returns>
    public static Vector2 ToVector2(this Direction4 dir4)
    {
        return dir4.ToDirection8().ToVector2();
    }
}