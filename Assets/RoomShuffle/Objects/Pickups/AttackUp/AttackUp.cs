using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporarily increases the player's attack power
/// </summary>
public class AttackUp : PickupScript
{
    public override void OnPickup()
    {
        Commons.PowerUpManager.GrantPowerUp(PowerUp.AttackUp);
    }
}
