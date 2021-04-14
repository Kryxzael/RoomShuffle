using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Causes a slow down effect of enemies when picked up
/// </summary>
public class SlowDown : PickupScript
{
    public override void OnPickup()
    {
        Commons.PowerUpManager.GrantPowerUp(PowerUp.SlowDown);
    }
}
