using RoomShuffle.Defaults;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Lets the player be hurt by a hurtbox
/// </summary>
[RequireComponent(typeof(MultiSoundPlayer))]
[RequireComponent(typeof(ParticleExplosion))]
public class PlayerHitbox : Hitbox
{
    private ParticleExplosion _exploder;
    private MultiSoundPlayer _multiSoundPlayer;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override HurtBoxTypes SusceptibleTo { get; } = HurtBoxTypes.HurtfulToPlayer;

    protected override void Awake()
    {
        base.Awake();
        _exploder = GetComponent<ParticleExplosion>();
        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();
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

        //Play sound
        _multiSoundPlayer.PlaySound();

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