using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Holds a set of enemies that can be spawned in a generated room
/// </summary>
[CreateAssetMenu(menuName = "Enemy Spawning Set")]
public class EnemySet : ScriptableObject
{
    [Header("Settings")]
    [Tooltip("Can this enemy set be used in eradication rooms")]
    public bool EnabledInEradication = true;

    [Header("Ground")]
    [Tooltip("The most common grounded enemy type")]
    public GameObject PrimaryGround;

    [Tooltip("The supplementary grounded enemy type")]
    public GameObject SecondaryGround;

    /* *** */

    [Header("Air")]
    [Tooltip("The most common air enemy type")]
    public GameObject PrimaryAir;

    [Tooltip("The supplementary air enemy type")]
    public GameObject SecondaryAir;
}
