using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GivePowerUpDebugPage : DebugPage
{
    public override string Header { get; } = "Give Powerup";

    protected override void RunItems(DebugMenu caller)
    {
        foreach (PowerUp i in typeof(PowerUp).GetEnumValues())
        {
            if (Toggle(i.ToString(), Commons.PowerUpManager.HasPowerUp(i)))
            {
                if (Commons.PowerUpManager.HasPowerUp(i))
                    Commons.PowerUpManager.RevokePowerup(i);

                else
                    Commons.PowerUpManager.GrantPermanentPowerup(i);
            }
        }
    }
}