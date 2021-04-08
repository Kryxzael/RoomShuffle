using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Marks a potential water- or lava-level
/// </summary>
public class FloodMarker : MonoBehaviour
{
    public int LevelIndex;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.right * 50f, transform.position + Vector3.right * 50f);
    }
}
