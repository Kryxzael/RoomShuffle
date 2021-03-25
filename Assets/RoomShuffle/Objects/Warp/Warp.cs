using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Lets the player warp from one point of the level to another
/// </summary>
public class Warp : MonoBehaviour
{
    [Tooltip("The ID of the warp. There should be two warps with the same ID in the level to link them up")]
    public int WarpID;

    /// <summary>
    /// Is the player currently in the area of the warp
    /// </summary>
    private bool _playerInWarp;

    private void Update()
    {
        if (_playerInWarp && Input.GetButtonDown("Interact"))
            WarpObject(this.GetPlayer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            _playerInWarp = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            _playerInWarp = false;
    }

    /// <summary>
    /// Warps the provided object to the target warp
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    public bool WarpObject(GameObject obj)
    {
        var target = GetTargetWarp();

        //No target warp was found
        if (!target)
            return false;

        //Warp object
        obj.transform.position = target.transform.position;
        return true;
    }

    /// <summary>
    /// Gets the other warp with the same ID as this one
    /// </summary>
    /// <returns></returns>
    public Warp GetTargetWarp()
    {
        return FindObjectsOfType<Warp>()
            .Where(i => i != this && i.WarpID == WarpID)
            .FirstOrDefault();
    }
}
