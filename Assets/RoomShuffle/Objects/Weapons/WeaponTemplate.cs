using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// The template of a weapon that defines its base stats and firing mechanics
/// </summary>
public abstract class WeaponTemplate : ScriptableObject
{
    [Tooltip("The weapon's base damage")]
    public RandomValueFromSet BaseDamage;

    [Tooltip("gets or sets the maximum durability value of the weapon")]
    public RandomValueFromSet MaxDurability;

    [Tooltip("Gets the amount of time (in seconds) the player must wait until they can re-fire the weapon")]
    public float Cooldown;

    /* *** */

    /// <summary>
    /// Gets whether the weapon can currently be fired
    /// </summary>
    /// <returns></returns>
    public virtual bool CanFire(WeaponInstance instance)
    {
        return instance.RemainingCooldown == 0f && instance.Durability > 0;
    } 

    /// <summary>
    /// Fires the weapon from the provided weapon shooter
    /// </summary>
    public void Fire(WeaponInstance instance, WeaponShooter shooter)
    {
        OnFire(shooter, shooter.CurrentAimingDirection);
        instance.Durability--;
        instance.LastFireTime = DateTime.Now;
    }

    /// <summary>
    /// Creates a weapon instance from the template
    /// </summary>
    /// <returns></returns>
    public virtual WeaponInstance CreateWeaponInstance()
    {
        WeaponInstance weapon = new WeaponInstance()
        {
            Template = this,
            MaxDurability = (int)MaxDurability.Pick(),
            BaseDamage = (int)BaseDamage.Pick()
        };

        weapon.Durability = weapon.MaxDurability;

        return weapon;
    }

    /// <summary>
    /// Fires the weapon from the provided weapon shooter
    /// </summary>
    /// <param name="direction"></param>
    protected abstract void OnFire(WeaponShooter shooter, Vector2 direction);    
}
