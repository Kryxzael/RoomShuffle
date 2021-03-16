using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.RoomShuffle.Objects.Health.HurtBoxes
{
    public class EnemyContactHurtbox : HurtBox
    {
        public int ContactBaseDamage;

        public override int GetDamage()
        {
            return Commons.EnemyProgression.GetScaledDamage(ContactBaseDamage);
        }

        public override HurtBoxTypes GetTargets()
        {
            return HurtBoxTypes.HurtfulToPlayer;
        }
    }
}
