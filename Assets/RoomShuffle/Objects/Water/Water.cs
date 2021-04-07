using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Represents water that the player can swim in
/// </summary>
public class Water : MonoBehaviour
{
    private static HashSet<Rigidbody2D> _bodiesInWater = new HashSet<Rigidbody2D>();

    /* *** */

    [Tooltip("By how much gravity is multiplied when a rigidbody is in water")]
    [Range(0f, 1f)]
    public float GravityMultiplier = 0.5f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.attachedRigidbody && !IsSubmerged(collision.attachedRigidbody))
        {
            collision.attachedRigidbody.gravityScale *= GravityMultiplier;
            _bodiesInWater.Add(collision.attachedRigidbody);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody && IsSubmerged(collision.attachedRigidbody))
        {
            collision.attachedRigidbody.gravityScale /= GravityMultiplier;
            _bodiesInWater.Remove(collision.attachedRigidbody);
        }
    }

    /// <summary>
    /// Gets whether the provided rigidbody is in a body of water
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public static bool IsSubmerged(Rigidbody2D body)
    {
        return _bodiesInWater.Contains(body);
    }
}
