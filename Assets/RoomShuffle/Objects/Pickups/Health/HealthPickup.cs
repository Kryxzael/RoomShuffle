using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Restores the player's health when picked up
/// </summary>
public class HealthPickup : PickupScript
{
    [Tooltip("How much HP the pickup will restore")]
    public int Restoration = 100;

    public override void OnPickup()
    {
        Commons.PlayerHealth.Heal(Restoration);
    }
}
