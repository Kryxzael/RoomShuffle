using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Replaces the object with a random item chosen from a list of candidates
/// </summary>
public class RandomItem : MonoBehaviour
{
    [Tooltip("The candidates for replacement")]
    public List<ItemWithSpawnrate> Candidates;

    /// <summary>
    /// Spawns an item, then destroys itself
    /// </summary>
    private void Start()
    {
        if (!Candidates.Any() || Candidates.Max(i => i.Spawnrate) == 0)
            throw new InvalidOperationException("No accessible candidates");

        ItemWithSpawnrate candidate;

        do
        {
            candidate = Candidates[Commons.RoomGenerator.RoomRng.Next(Candidates.Count)];
        } while (Commons.RoomGenerator.RoomRng.NextDouble() > candidate.Spawnrate);

        Instantiate(candidate.Item, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    [Serializable]
    public class ItemWithSpawnrate
    {
        [Range(0f, 1f)]
        [Tooltip("The spawn chance of the item")]
        public float Spawnrate = 1f;

        [Tooltip("The item to spawn")]
        public GameObject Item;
    }
}
