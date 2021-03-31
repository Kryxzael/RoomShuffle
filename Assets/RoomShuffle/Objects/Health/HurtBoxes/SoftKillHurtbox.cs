using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A hurtbox that soft-kills the player and kills anything else
/// </summary>
public class SoftKillHurtbox : HurtBox
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override bool IgnoresInvincibilityFrames => true;

    [Tooltip("Does the respawning logic of this hurtbox ignore god mode")]
    public bool RespawnIgnoresGodMode;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public override int GetDamage(Hitbox target)
    {
        //Players should be soft-killed
        if (target is PlayerHitbox)
        {
            return Commons.PlayerHealth.GetSoftDeathDamage();
        }

        //Anything else should just be killed
        return int.MaxValue;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override HurtBoxTypes GetTargets()
    {
        return HurtBoxTypes.HurtfulToEnemies | HurtBoxTypes.HurtfulToPlayer;
    }

    public override void OnDealDamage(Hitbox hitbox)
    {
        if (hitbox is PlayerHitbox && (Cheats.HealthCheat != Cheats.HealthCheatType.Godmode || !RespawnIgnoresGodMode))
        {
            Commons.RespawnPlayer();
        }
    }
}
