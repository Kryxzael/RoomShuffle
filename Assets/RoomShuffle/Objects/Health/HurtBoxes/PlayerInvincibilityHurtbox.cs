using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The hurtbox the player has that is active when the player is invincible
/// </summary>
public class PlayerInvincibilityHurtbox : Hurtbox
{
    public override bool IgnoresInvincibilityFrames => true;
    public override bool ContinuousDamage => true;

    public override int GetDamage(Hitbox target)
    {
        //Infinite damage when invincible
        if (Commons.PowerUpManager.HasPowerUp(PowerUp.Invincibility))
            return int.MaxValue;

        //No damage otherwise
        return 0;
    }

    public override HurtBoxTypes GetTargets()
    {
        return HurtBoxTypes.HurtfulToEnemies;
    }
}