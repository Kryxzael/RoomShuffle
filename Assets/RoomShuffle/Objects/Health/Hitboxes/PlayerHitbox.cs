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
    private ExplodeOnDeath _exploder;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override HurtBoxTypes SusceptibleTo { get; } = HurtBoxTypes.HurtfulToPlayer;

    protected override void Awake()
    {
        _exploder = GetComponent<ExplodeOnDeath>();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="hurtbox"></param>
    protected override void OnReceiveDamage(HurtBox hurtbox)
    {
        //Dead men don't scream
        if (Commons.PlayerHealth.IsDead)
            return;

        //Hitboxes are not active when noclip is enabled
        if (Cheats.Noclip)
            return;

        GrantInvincibilityFrames();
        Commons.PlayerHealth.DealDamage(hurtbox.GetDamage(this));

        //TODO: Move this?
        if (Commons.PlayerHealth.IsDead)
        {
            if (_exploder)
                _exploder.ExplodeBig();
        }
        else
        {
            if (_exploder && !hurtbox.IgnoresInvincibilityFrames)
                _exploder.ExplodeSmall();
        }
    }
}