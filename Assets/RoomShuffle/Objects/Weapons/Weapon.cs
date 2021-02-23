using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [Tooltip("Gets or sets the range of damage the weapon does")]
    public RandomValueBetween Damage;

    [Header("Gets the amount of time (in seconds) the player must wait until they can re-fire the weapon")]
    public float Cooldown;

    /// <summary>
    /// Gets the amount of seconds before the weapon can fire again
    /// </summary>
    public float RemainingCooldown
    {
        get
        {
            return Math.Max(0f, Cooldown - (float)(DateTime.Now - _lastFireTime).TotalSeconds);
        }
    }

    [Header("Durability")]
    [Tooltip("gets or sets the maximum durability value of the weapon")]
    public int MaxDurability;

    [Tooltip("Gets or sets the weapon's current durability value")]
    public int Durability;

    /* *** */

    private DateTime _lastFireTime;

    /// <summary>
    /// Gets whether the weapon can currently be fired
    /// </summary>
    /// <returns></returns>
    public virtual bool CanFire()
    {
        return RemainingCooldown == 0f && Durability > 0;
    } 

    /// <summary>
    /// Fires the weapon from the provided weapon shooter
    /// </summary>
    public void Fire(WeaponShooter shooter)
    {
        OnFire(shooter, shooter.CurrentAimingDirection);
        Durability--;
        _lastFireTime = DateTime.Now;
    }

    /// <summary>
    /// Fires the weapon from the provided weapon shooter
    /// </summary>
    /// <param name="direction"></param>
    protected abstract void OnFire(WeaponShooter shooter, Vector2 direction);    
}
