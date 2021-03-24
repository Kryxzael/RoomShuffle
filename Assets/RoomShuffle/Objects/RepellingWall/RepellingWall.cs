using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using UnityEngine;

public class RepellingWall : MonoBehaviour
{
    private static HashSet<RepellingWall> _wallsDetectingThePlayer = new HashSet<RepellingWall>();

    /// <summary>
    /// Gets whether the player is currently touching a repelling area
    /// </summary>
    public static bool PlayerIsInRepellingArea
    {
        get
        {
            return _wallsDetectingThePlayer
                .Where(i => i) // (i != null)
                .Any();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            _wallsDetectingThePlayer.Add(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            _wallsDetectingThePlayer.Remove(this);
    }

}
