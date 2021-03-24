using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A simple hurtbox that damages all types of objects
/// </summary>
public class GlobalHurtbox : HurtBox
{
    [Tooltip("The amount of damage the hurtbox will do")]
    public int Damage;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override int GetDamage()
    {
        return Damage;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override HurtBoxTypes GetTargets()
    {
        return HurtBoxTypes.HurtfulToEnemies | HurtBoxTypes.HurtfulToPlayer;
    }
}
