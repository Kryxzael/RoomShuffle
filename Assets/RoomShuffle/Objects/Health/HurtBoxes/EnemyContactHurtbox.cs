using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A hurtbox that deals damage to the player when it comes in contact with it
/// </summary>
public class EnemyContactHurtbox : HurtBox
{
    /// <summary>
    /// The base damage the hurtbox will deal
    /// </summary>
    public int ContactBaseDamage;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override bool IgnoresInvincibilityFrames => false;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override int GetDamage(Hitbox target)
    {
        return Commons.EnemyProgression.GetScaledDamage(ContactBaseDamage);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override HurtBoxTypes GetTargets()
    {
        return HurtBoxTypes.HurtfulToPlayer;
    }
}