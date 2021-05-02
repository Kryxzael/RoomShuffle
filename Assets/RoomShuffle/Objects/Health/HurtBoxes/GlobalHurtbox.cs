using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A simple hurtbox that damages all types of objects
/// </summary>
public class GlobalHurtbox : Hurtbox
{
    [Tooltip("The amount of damage the hurtbox will do")]
    public int Damage;

    public int OverridePlayerDamage = -1;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override bool IgnoresInvincibilityFrames => false;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override bool ContinuousDamage => true;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override int GetDamage(Hitbox target)
    {
        if (target is PlayerHitbox)
        {
            if (OverridePlayerDamage != -1)
                return Commons.EnemyProgression.GetScaledDamage(OverridePlayerDamage);

            return Commons.EnemyProgression.GetScaledDamage(Damage);
        }
            

        return Commons.PlayerProgression.GetScaledDamage(Damage);
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
