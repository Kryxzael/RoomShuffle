using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Sets the player's health when the object spawns
/// </summary>
public class SetPlayerHealth : MonoBehaviour
{
    [Tooltip("The health the player will get when the object spawns")]
    public int PlayerHealth = 50;

    private void Start()
    {
        Commons.PlayerHealth.Health = PlayerHealth;
    }
}