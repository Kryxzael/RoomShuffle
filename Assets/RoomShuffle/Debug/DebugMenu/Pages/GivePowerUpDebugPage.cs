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
            if (Button(i.ToString()))
            {
                Commons.PowerUpManager.GrantPowerUp(i);
            }
        }
    }
}