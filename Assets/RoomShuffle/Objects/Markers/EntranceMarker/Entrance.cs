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
        Instantiate(Player, transform.position, Quaternion.identity, transform.parent);
    }
}
