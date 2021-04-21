using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grants a power-up when picked up
/// </summary>
public class PowerUpPickup : PickupScript
{
    [Tooltip("The power-up to grant")]
    public PowerUp PowerUp;

    public override void OnPickup()
    {
        Commons.PowerUpManager.GrantPowerUp(PowerUp);
    }
}
