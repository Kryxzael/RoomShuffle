using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Turns a flippable around when it walks into a wall
/// </summary>
[RequireComponent(typeof(Flippable), typeof(Collider2D))]
[DisallowMultipleComponent]
public class TurnOnWall : MonoBehaviour
{
    //The distance to raycast for wall checks
    private const float CHECK_DISTANCE = 0.02f;

    //The smallest angle a surface can have to be considered a wall
    private const float MIN_WALL_DEGREE = 40f;

    /* *** */

    [Header("Turn on Wall")]
    [Range(0f, 1f)]
    [Tooltip("Adjusts the top and bottom rays during the raycast closer to the center. Increasing this value may fix objects turning around when reaching an upwards slope")]
    public float SlopeDetectionThreshold = 0.25f;

    /* *** */

    protected Flippable _flippable;
    protected Collider2D _collider;

    protected virtual void Awake()
    {
        _flippable = GetComponent<Flippable>();
        _collider = GetComponent<Collider2D>();
    }

    private void LateUpdate()
    {
        //Flip if on wall
        if (OnWall())
            _flippable.Flip();
    }

    public bool OnWall()
    {
        /*
         * Gets the coordinates of the sides of the object's hit box
         */
        float left = _collider.bounds.min.x;
        float right = _collider.bounds.max.x;

        float bottom = _collider.bounds.min.y + _collider.bounds.size.y * 0.5f * SlopeDetectionThreshold;
        float center = _collider.bounds.center.y;
        float top = _collider.bounds.max.y - _collider.bounds.size.y * 0.5f * SlopeDetectionThreshold;

        //The x-coordinate to start raycasts at and the direction to cast in.
        float vectorX;
        Vector2 direction;

        //Configures the cast to check for walls on the right
        if (_flippable.Direction == Direction1D.Right)
        {
            vectorX = right;
            direction = Vector2.right;
        }

        //Configures the cast to check for walls on the left
        else if (_flippable.Direction == Direction1D.Left)
        {
            vectorX = left;
            direction = Vector2.left;
        }

        //Invalid
        else
        {
            Debug.LogWarning("Flippable does not have a valid direction");
            return false;
        }

        /*
         * Performs the ray casts
         */

        //Stores any potential hit
        RaycastHit2D hit;

        //Checks the top, center and bottom of the of the object's hitbox
        if (hit = RaycastAndDebug(vectorX, top, direction))
        {
            if (HitIsWall(hit))
                return true;
        }

        if (hit = RaycastAndDebug(vectorX, center, direction))
        {
            if (HitIsWall(hit))
                return true;
        }

        if (hit = RaycastAndDebug(vectorX, bottom, direction))
        {
            if (HitIsWall(hit))
                return true;
        }

        return false;
    }

    private static RaycastHit2D RaycastAndDebug(float x, float y, Vector2 direction)
    {
        Debug.DrawLine(new Vector2(x, y), new Vector2(x, y) + direction * CHECK_DISTANCE, Color.green);
        return Physics2D.Raycast(new Vector2(x, y), direction, CHECK_DISTANCE, Commons.Masks.GroundAndBlockers);
    }

    /// <summary>
    /// Checks if the provided raycast hit is determined to be a wall
    /// </summary>
    /// <param name="hit"></param>
    private bool HitIsWall(RaycastHit2D hit)
    {
        //Gets the angle from the hit's normal
        float angle = hit.normal.RealAngleBetween(Vector2.up);
        angle = Math.Abs(angle);

        //The hit is a wall if it's normal angle is greater than the minimum set wall degree
        return angle > MIN_WALL_DEGREE;
    }
}
