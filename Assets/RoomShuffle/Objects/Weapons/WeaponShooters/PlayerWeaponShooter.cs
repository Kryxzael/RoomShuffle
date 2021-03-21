using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Represents the aiming and shooter functionality of the player
/// </summary>
[RequireComponent(typeof(Flippable))]
public class PlayerWeaponShooter : WeaponShooterBase
{
    private Flippable _flippable;

    private void Awake()
    {
        _flippable = GetComponent<Flippable>();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override Vector2 GetCurrentAimingDirection()
    {
        float verticalAxis = Input.GetAxisRaw("Vertical");

        if (verticalAxis > 0f)
            return Vector2.up;

        else if (verticalAxis < 0f)
            return Vector2.down;

        return _flippable.DirectionVector;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override Vector2 GetProjectilesSpawnPoint()
    {
        return transform.position;
    }
}
