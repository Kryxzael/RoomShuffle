using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

//Note about this being a two-purpose script
//There's been some issues with edge detection by walls, and it's easier and more optimized to just do it like this
//This means that an object can't be edge detecting whilst NOT being wall detecting, but this shouldn't be a problem

/// <summary>
/// Turns a flippable object around when its about to walk off an edge
/// </summary>
[RequireComponent(typeof(Flippable), typeof(Collider2D))]
[DisallowMultipleComponent]
public class TurnOnWallAndEdge : TurnOnWall
{
    [Tooltip("The maximum height the pit can have to be considered a pit")]
    public float MaxPitHeight = 1.5f;

    [Tooltip("Adjusts how close the raycast is done to the object. Decreasing this value may fix objects turning around when reaching a downwards slope")]
    public float SlopeDetectionDistance = 0.25f;

    protected override void Awake()
    {
        //Note about overriding Awake:
        //This is necessary because Unity won't run parent classes' message functions. (Case in point: TurnOnWall.Update() never runs from this script)
        //We run base.Awake() manually to set up component references
        base.Awake();
    }

    private void Update()
    {
        //Flip if on wall or edge
        if (OnWall() || OnEdge())
            _flippable.Flip();
    }

    public bool OnEdge()
    {
        //If the object is not grounded, it should not care about edges (there will be edges on all sides and it will freak out)
        if (!this.OnGround2D())
            return false;

        /*
         * Gets the position to start the raycast from
         * This is either at the bottom left or bottom right side of the hitbox of the object (plus some padding determined by SlopeDetectionDistance)
         */
        float x;
        float y = _collider.bounds.center.y;

        //Configures the cast to check for pits on the left
        if (_flippable.Direction == Direction1D.Left)
        {
            x = _collider.bounds.min.x - SlopeDetectionDistance;
        }

        //Configures the cast to check for pits on the right
        else if (_flippable.Direction == Direction1D.Right)
        {
            x = _collider.bounds.max.x + SlopeDetectionDistance;
        }

        //Invalid
        else
        {
            Debug.LogWarning("Flippable does not have a valid direction");
            return false;
        }

        /*
         * Performs the ray cast and returns the result
         */
        Vector2 castPos = new Vector2(x, y);
        float castDistance = MaxPitHeight + _collider.bounds.extents.y;

        Debug.DrawLine(castPos, castPos + Vector2.down * castDistance, Color.green);
        return !Physics2D.Raycast(castPos, Vector2.down, castDistance);
    }
}
