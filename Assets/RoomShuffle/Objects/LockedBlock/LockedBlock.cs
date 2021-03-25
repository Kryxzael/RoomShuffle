using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A block that is destroyed if the player touches it with a key
/// </summary>
public class LockedBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.IsPlayer() && Commons.Inventory.PuzzleKeys > 0)
        {
            Commons.Inventory.PuzzleKeys--;
            Destroy(gameObject);
        }
    }
}
