using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A weapon instance created from a weapon template as it appears in the world
/// </summary>
public class WeaponInstance
{
    [Tooltip("The template base of the weapon")]
    public WeaponTemplate Template;

    /*
     * Configuration
     */

    [Tooltip("The weapon's maximum durability as chosen from the template upon its creation")]
    public int MaxDurability;

    [Tooltip("The weapon's base damage as chosen from the template upon its creation")]
    public int BaseDamage;

    /*
     * Status
     */

    [Tooltip("The weapon's current durability value")]
    public int Durability;

    /* *** */

    /// <summary>
    /// Gets the amount of seconds before the weapon can fire again
    /// </summary>
    public float RemainingCooldown
    {
        get
        {
            return Math.Max(0f, Template.Cooldown - (float)(DateTime.Now - LastFireTime).TotalSeconds);
        }
    }

    /// <summary>
    /// The time the weapon was last fired at
    /// </summary>
    public DateTime LastFireTime;

    /// <summary>
    /// Creates a new weapon instance. This should only be done from WeaponTemplate.CreateWeaponInstance
    /// </summary>
    /// <param name="caller"></param>
    public WeaponInstance([CallerMemberName] string caller = "")
    {
        if (caller != nameof(WeaponTemplate.CreateWeaponInstance))
        {
            Debug.LogWarning("Weapon instances should only be created from WeaponTemplate.CreateWeaponInstance");
        }
    }

    /// <summary>
    /// Gets whether the weapon can currently be fired
    /// </summary>
    /// <returns></returns>
    public bool CanFire()
    {
        return Template.CanFire(this);
    }

    /// <summary>
    /// Fires the weapon from the provided weapon shooter
    /// </summary>
    /// <param name="shooter"></param>
    public void Fire(WeaponShooter shooter)
    {
        Template.Fire(this, shooter);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Template.ToString();
    }
}
