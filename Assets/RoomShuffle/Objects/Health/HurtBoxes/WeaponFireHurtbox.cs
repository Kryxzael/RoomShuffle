using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The hurtbox on a projectile or other weapon attack type
/// </summary>
public class WeaponFireHurtbox : HurtBox
{
    /// <summary>
    /// The object that shot the hurtbox
    /// </summary>
    public WeaponShooterBase Shooter { get; set; }

    /// <summary>
    /// The weapon that was fired
    /// </summary>
    public WeaponInstance Weapon { get; set; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override int GetDamage()
    {
        ProgressionController controller;

        //Determine who to scale damage by based on who fired the weapon
        if (Shooter.IsPlayer())
            controller = Commons.PlayerProgression;
        else
            controller = Commons.EnemyProgression;

        //Upscale damage
        return controller.GetScaledDamage(Weapon.BaseDamage);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override HurtBoxTypes GetTargets()
    {
        //The player hurts enemies...
        if (Shooter.IsPlayer())
            return HurtBoxTypes.HurtfulToEnemies;

        //...and vice versa
        return HurtBoxTypes.HurtfulToPlayer;
    }
}
