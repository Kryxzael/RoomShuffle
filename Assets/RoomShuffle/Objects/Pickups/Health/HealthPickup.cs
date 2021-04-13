using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Restores the player's health when picked up
/// </summary>
public class HealthPickup : PickupScript
{
    [Range(0f, 1f)]
    [Tooltip("How much HP the pickup will restore")]
    public float RestorationPercentage = 0.25f;

    public override void OnPickup()
    {
        Commons.PlayerHealth.Heal((int)Mathf.Ceil(Commons.PlayerHealth.MaximumHealth * RestorationPercentage));
    }
}
