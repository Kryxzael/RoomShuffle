using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the entrance point of a room
/// </summary>
public class Entrance : MonoBehaviour
{
    public GameObject Player;

    /// <summary>
    /// Spawns a player at the entrance's position
    /// </summary>
    public void SpawnPlayer()
    {
        const float NUDGE = 0.25f;

        Commons.InstantiateInCurrentLevel(Player, transform.position + Vector3.up * NUDGE);
    }
}
