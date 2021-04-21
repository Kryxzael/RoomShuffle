using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The hurtbox the player has that is active when the player is invincible
/// </summary>
public class PlayerInvincibilityHurtbox : HurtBox
{
    public override bool IgnoresInvincibilityFrames => true;
    public override bool ContinuousDamage => true;

    public override int GetDamage(Hitbox target)
    {
        if (Commons.PowerUpManager.HasPowerUp(PowerUp.Invincibility))
            return int.MaxValue;

        return 0;
    }

    public override HurtBoxTypes GetTargets()
    {
        return HurtBoxTypes.HurtfulToEnemies;
    }
}