using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Handles a weapon user
/// </summary>
public abstract class WeaponShooterBase : MonoBehaviour
{
    /// <summary>
    /// Gets the direction the shooter is currently aiming in
    /// </summary>
    public abstract Vector2 GetCurrentAimingDirection();


    /// <summary>
    /// Gets the position where a new projectile should be spawned based on the aiming direction
    /// </summary>
    /// <returns></returns>
    public abstract Vector2 GetProjectilesSpawnPoint();
}
