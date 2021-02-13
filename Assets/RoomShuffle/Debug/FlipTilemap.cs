using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class FlipTilemap : MonoBehaviour
{
    private void Start()
    {
        Tilemap map = GetComponent<Tilemap>();
        Debug.Log(map.cellBounds);
    }
}
