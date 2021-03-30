using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the entrance point of a room
/// </summary>
public class Entrance : MonoBehaviour
{
    public GameObject Player;

    private void Start()
    {
        SpawnPlayer();
    }

    /// <summary>
    /// Spawns a player at the entrance's position
    /// </summary>
    public void SpawnPlayer()
    {
        Commons.InstantiateInCurrentLevel(Player, transform.position);
    }
}
