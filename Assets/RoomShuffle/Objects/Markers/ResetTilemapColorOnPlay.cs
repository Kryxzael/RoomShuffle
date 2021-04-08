using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Resets design colors on play mode
/// </summary>
[RequireComponent(typeof(Tilemap))]
public class ResetTilemapColorOnPlay : MonoBehaviour
{
    void Start()
    {
        GetComponent<Tilemap>().color = Color.white;
    }
}
