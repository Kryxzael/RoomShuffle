using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Levels up the players health
/// </summary>
public class HealthLevelPickup : Pickup
{
    protected override void OnPickup()
    {
        //add one to health level
        Commons.PlayerProgression.LevelUpHealth();

    }
}

