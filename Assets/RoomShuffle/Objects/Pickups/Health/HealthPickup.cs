using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Restores the player's health when picked up
/// </summary>
public class HealthPickup : Pickup
{
    [Tooltip("How much HP the pickup will restore")]
    public int Restoration = 100;

    protected override void OnPickup()
    {
        Commons.PlayerHealth.Heal(Restoration);
    }
}
