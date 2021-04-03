using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A pickup that grants the player a general key
/// </summary>
public class AddTimePickup : Pickup
{

    public float AddTime;

    public Timer CountdownTimer;
    protected override void OnPickup()
    {
        CountdownTimer.AddTime(AddTime);
    }
}
