using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.RoomShuffle.Objects.Health.HurtBoxes
{
    public class WeaponFireHurtbox : HurtBox
    {
        public WeaponShooterBase Shooter { get; set; }
        public WeaponInstance Weapon { get; set; }

        public override int GetDamage()
        {
            ProgressionController controller;

            if (Shooter.IsPlayer())
                controller = Commons.PlayerProgression;
            else
                controller = Commons.EnemyProgression;

            return controller.GetScaledDamage(Weapon.BaseDamage);
        }

        public override HurtBoxTypes GetTargets()
        {
            if (Shooter.IsPlayer())
                return HurtBoxTypes.HurtfulToEnemies;

            return HurtBoxTypes.HurtfulToPlayer;
        }
    }
}
