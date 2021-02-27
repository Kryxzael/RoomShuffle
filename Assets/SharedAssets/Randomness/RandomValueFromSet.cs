using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A value that will be determined by weighted randomness
/// </summary>
[Serializable]
public class RandomValueFromSet
{
    [Tooltip("The candidates of the random value set. There must be at least one item with a non-zero pickrate")]
    public List<WeightedRandomItem> Candidates = new List<WeightedRandomItem>();

    /// <summary>
    /// Picks a random item
    /// </summary>
    /// <returns></returns>
    public float Pick()
    {
        return Pick(new System.Random(UnityEngine.Random.Range(0, int.MaxValue)));
    }

    /// <summary>
    /// Picks a random item from the set with the provided RNG
    /// </summary>
    /// <param name="rng"></param>
    /// <returns></returns>
    public float Pick(System.Random rng)
    {
        //If there are no valid candidates, throw.
        if (Candidates == null || !Candidates.Any() || Candidates.Sum(i => i.Pickrate) == 0)
            throw new InvalidOperationException("No accessible candidates");


        /*
         * Standard weighted randomness operation
         */
        WeightedRandomItem candidate;

        do
        {
            candidate = Candidates[rng.Next(Candidates.Count)];
        } while (rng.NextDouble() > candidate.Pickrate);

        return candidate.Item;
    }

    /// <summary>
    /// Represents an item with a pickrate
    /// </summary>
    [Serializable]
    public struct WeightedRandomItem
    {
        [Tooltip("The item with a pickrate")]
        public float Item;

        [Tooltip("The pickrate of the item")]
        [Range(0f, 1f)]
        public float Pickrate;

        /// <summary>
        /// Creates a new Weighted Random Item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pickrate"></param>
        public WeightedRandomItem(float item, float pickrate)
        {
            Item = item;
            Pickrate = pickrate;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Item} ({Pickrate:P})";
        }
    }
}
