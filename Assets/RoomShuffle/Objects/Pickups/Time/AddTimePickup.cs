using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A pickup that grants the player a general key
/// </summary>
public class AddTimePickup : PickupScript
{
    [Tooltip("The amount of seconds to add to the timer")]
    public float AddTime;

    [Tooltip("The timer to add seconds to")]
    public Timer CountdownTimer; //TODO: This REALLY shouldn't be a reference here!

    public override void OnPickup()
    {
        CountdownTimer.AddTime(AddTime);
    }
}
