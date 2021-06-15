using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Represents a snapshot of the player's position at a particular time
/// </summary>
[Serializable]
public struct PositionSnapshot
{
    /// <summary>
    /// Gets the position of the object at the snapshot
    /// </summary>
    public Vector2 Position { get; }

    /// <summary>
    /// Gets the direction of the object at the snapshot
    /// </summary>
    public Direction1D Direction { get; }

    /// <summary>
    /// Gets the time the snapshot was taken
    /// </summary>
    public DateTime Time { get; }

    /// <summary>
    /// Gets the animation the player was in when the snapshot was taken
    /// </summary>
	public SpriteAnimation Animation { get; }

	private PositionSnapshot(Vector2 pos, Direction1D dir, DateTime time, SpriteAnimation animation)
    {
        Position = pos;
        Direction = dir;
        Time = time;
		Animation = animation;
	}

    /// <summary>
    /// Extracts a new position snapshot from the provided object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static PositionSnapshot FromObjects(Flippable obj, SpriteAnimator animator = null)
    {
        SpriteAnimation animation = null;

        if (animator)
            animation = animator.Animation;

        return new PositionSnapshot(obj.transform.position, obj.Direction, DateTime.Now, animation);
    }
}
