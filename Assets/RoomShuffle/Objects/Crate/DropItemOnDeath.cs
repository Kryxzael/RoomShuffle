using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Allows an object to drop an item remotely
/// </summary>
public class DropItemOnDeath : MonoBehaviour
{
    [Tooltip("The item to drop")]
    public GameObject DroppedItemPrefab;

    /// <summary>
    /// Drops the item
    /// </summary>
    public void DropItem()
    {
        Commons.InstantiateInCurrentLevel(DroppedItemPrefab, transform.position);
    }
}