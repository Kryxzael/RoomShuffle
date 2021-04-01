using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Allows an object to drop an item remotely
/// </summary>
public class DropLootTableOnDeath : MonoBehaviour
{
    [Tooltip("The radius around the transform of the object that items can be spawned at")]
    public float SpawningRadius = 1f;

    [Tooltip("The loot tables the dropped i")]
    public List<LootTableWithSpawnrate> LootTables = new List<LootTableWithSpawnrate>();

    /// <summary>
    /// Drops the item
    /// </summary>
    public void DropItem()
    {
        /*
         * Choose a random loot table
         */
        LootTableWithSpawnrate candidate;

        if (LootTables == null || !LootTables.Any(i => i.Spawnrate > 0f))
            throw new System.InvalidOperationException("DropItemOnDeath has no accessible loot tables");

        do
        {
            candidate = LootTables[Random.Range(0, LootTables.Count)];
        } while (Random.value > candidate.Spawnrate);


        /*
         * Spawn items of selected loot table
         */

        foreach (var i in candidate.Table.GetLootItems())
            Commons.InstantiateInCurrentLevel(i, transform.Position2D() + Random.insideUnitCircle * SpawningRadius);
    }
}