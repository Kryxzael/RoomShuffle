using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Lets the player be hurt by a hurtbox
/// </summary>
public class PlayerHitbox : Hitbox
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override HurtBoxTypes SusceptibleTo { get; } = HurtBoxTypes.HurtfulToPlayer;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="hurtbox"></param>
    protected override void OnReceiveDamage(HurtBox hurtbox)
    {
        //Hitboxes are not active when noclip is enabled
        if (Cheats.Noclip)
            return;

        GrantInvincibilityFrames();
        Commons.PlayerHealth.DealDamage(hurtbox.GetDamage(this));
    }
}