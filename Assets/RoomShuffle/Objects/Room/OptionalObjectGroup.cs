﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Marker class for a group of children where only one object will be spawned
/// </summary>
public class OptionalObjectGroup : MonoBehaviour
{
    [Tooltip("The chance the generator has to spawn none of the children")]
    [Range(0f, 1f)]
    public float ChanceToSpawnNothing;

    private void Start()
    {
        if (GetCandidates().Count() < 2)
            throw new InvalidOperationException("An OptionalObjectGroup must have at least two direct OptionalObjectCandidates");
    }

    /// <summary>
    /// Gets the OptionalObjectCandidates of the direct children of this object
    /// </summary>
    /// <returns></returns>
    private IEnumerable<OptionalObjectCandidate> GetCandidates()
    {
        return transform

            //Transform is IEnumerable (of its direct children), but not IEnumerable<T>. We need to cast it to unlock LINQ functions
            .Cast<Transform>()

            //Get the OptionalObjectCandidate of every direct child
            .Select(i => i.GetComponent<OptionalObjectCandidate>())

            //If we find a child that doesn't have an OptionalObjectCandidate, we exclude it
            .Where(i => i != null);
    }

    /// <summary>
    /// Selects one of the OptionalObjectCandidates in the 
    /// </summary>
    /// <param name="random"></param>
    public void Select(System.Random random)
    {
        //Gets all candidates (direct children)
        var candidates = GetCandidates().ToArray();

        //No accessible candidates found in group
        if (candidates.All(i => i.SpawnChance == 0f))
            throw new System.InvalidOperationException($"OptionalSpawnGroup has no accessible candidates");

        //Choose a random object to spawn from the group
        OptionalObjectCandidate currentCandidate;

        do
        {
            currentCandidate = candidates[random.Next(candidates.Length)];
        } while (random.NextDouble() > currentCandidate.SpawnChance);

        //Disable all other objects
        foreach (var i in candidates)
        {
            if (i == currentCandidate)
                continue;

            i.gameObject.SetActive(false);
        }
    }
}
