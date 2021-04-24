using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Levels up the player's damage power
/// </summary>
public class DamageLevelPickup : PickupScript
{
    public override void OnPickup()
    {
        //Add one to health level
        Commons.PlayerProgression.LevelUpDamage();
    }
}
