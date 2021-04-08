using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaserHitbox : HurtBox
{
    [Tooltip("If the laser should hurt the player")]
    public bool FriendlyFireOn;
    
    [Tooltip("The amount of base damage the hurtbox will do")]
    public int BaseDamage;

    [Tooltip("The hurtbox of the projectile")]
    public WeaponFireHurtbox _hurtBox;


    private bool ShooterIsPlayer = true;
    private ProgressionController _controller;

    private void Start()
    {
        
        //projectile not found
        if (!_hurtBox)
        {
            _controller = Commons.PlayerProgression;
            ShooterIsPlayer = true;
            return;
        }

        //Check who shot the projectile
        if (_hurtBox.Shooter.IsPlayer())
        {
            _controller = Commons.PlayerProgression;
            ShooterIsPlayer = true;
        }
        else
        {
            _controller = Commons.EnemyProgression;
            ShooterIsPlayer = false;
        }
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override bool IgnoresInvincibilityFrames => true;

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
        return _controller.GetScaledDamage(BaseDamage);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override HurtBoxTypes GetTargets()
    {
        if (FriendlyFireOn)
        {
            return HurtBoxTypes.HurtfulToEnemies | HurtBoxTypes.HurtfulToPlayer;
        }

        if (ShooterIsPlayer)
        {
            return HurtBoxTypes.HurtfulToEnemies;
        }
        else
        {
            return HurtBoxTypes.HurtfulToPlayer;
        }


    }
}
