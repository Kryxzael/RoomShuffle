using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A hurtbox that soft-kills the player and kills anything else
/// </summary>
public class SoftKillHurtbox : HurtBox
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override bool IgnoresInvincibilityFrames => true;

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
            Commons.RespawnPlayer();
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
}
