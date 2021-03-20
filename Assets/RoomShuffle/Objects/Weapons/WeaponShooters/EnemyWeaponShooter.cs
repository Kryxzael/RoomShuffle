using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Represents the aiming and shooter functionality of an enemy
/// </summary>
public class EnemyWeaponShooter : WeaponShooterBase 
{
    private Vector2 _aim;

    /// <summary>
    /// Sets the direction the enemy is aiming
    /// </summary>
    /// <param name="direction"></param>
    public void SetAim(Vector2 direction)
    {
        _aim = direction;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override Vector2 GetCurrentAimingDirection()
    {
        return _aim;
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
