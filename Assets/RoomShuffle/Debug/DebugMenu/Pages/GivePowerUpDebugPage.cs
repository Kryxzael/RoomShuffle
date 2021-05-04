using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A debug page that allows the player to give themselves a power-up
/// </summary>
public class GivePowerUpDebugPage : DebugPage
{
    public override string Header { get; } = "Give Powerup";

    protected override void RunItems(DebugMenu caller)
    {
        //Create a toggle for each power-up
        foreach (PowerUp i in typeof(PowerUp).GetEnumValues())
        {
            if (Toggle(i.ToString(), Commons.PowerUpManager.HasPowerUp(i)))
            {
                //Remove power-up if the player already has it
                if (Commons.PowerUpManager.HasPowerUp(i))
                    Commons.PowerUpManager.RevokePowerup(i);

                //Add the power up if the player doesn't have it
                else
                    Commons.PowerUpManager.GrantPermanentPowerup(i);
            }
        }
    }
}