using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A loot table with one or more custom objects
/// </summary>
[CreateAssetMenu(menuName = "Loot Tables/Custom Loot Table")]
public class CustomLootTable : LootTable
{
    [Tooltip("The loot of this table")]
    public List<GameObject> Loot = new List<GameObject>();

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<GameObject> GetLootItems()
    {
        return Loot;
    }
}
