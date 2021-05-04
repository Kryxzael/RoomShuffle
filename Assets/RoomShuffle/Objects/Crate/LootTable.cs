using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Implements a loot table, which is a collection of items that can be dropped by an enemy or crate
/// </summary>
public abstract class LootTable : ScriptableObject
{
    /// <summary>
    /// Gets the items contained in the loot table
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerable<GameObject> GetLootItems();
}
