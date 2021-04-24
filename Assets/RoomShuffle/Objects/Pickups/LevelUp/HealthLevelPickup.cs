using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Levels up the players health
/// </summary>
public class HealthLevelPickup : PickupScript
{
    public override void OnPickup()
    {
        //Add one to health level
        Commons.PlayerProgression.LevelUpHealth();
    }
}

