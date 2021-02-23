using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Handles a weapon user
/// </summary>
[RequireComponent(typeof(Flippable))]
public class WeaponShooter : MonoBehaviour
{
    /// <summary>
    /// Gets the direction the player is currently aiming in
    /// </summary>
    public Vector2 CurrentAimingDirection { get; private set; }

    public Bounds ProjectileSpawningBounds;

    /* *** */

    private Flippable _flippable;

    private void Awake()
    {
        _flippable = GetComponent<Flippable>();
    }

    private void Update()
    {
        float verticalAxis = Input.GetAxis("Vertical");

        if (verticalAxis > 0f)
            CurrentAimingDirection = Vector2.up;

        else if (verticalAxis < 0f)
            CurrentAimingDirection = Vector2.down;

        else
            CurrentAimingDirection = _flippable.DirectionVector;
    }

    /// <summary>
    /// Gets the position where a new projectile should be spawned based on the aiming direction
    /// </summary>
    /// <returns></returns>
    public Vector2 GetProjectilesSpawnPoint()
    {
        //TODO: Something here
        return transform.position;
    }
}
