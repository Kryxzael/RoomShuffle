using System;
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
        if (GetCandidates().Count() == 1)
            throw new InvalidOperationException("An OptionalObjectGroup must have at least two direct OptionalObjectCandidates or have none at all");
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
    /// Selects one of the OptionalObjectCandidates to spawn and destroys the others
    /// </summary>
    /// <param name="random"></param>
    public void Select(System.Random random)
    {
        //Gets all candidates (direct children)
        var allCandidates = GetCandidates();

        var availableCandidates = allCandidates
            .Where(i => i.SpawnWhenEntranceIs.HasFlag(Commons.RoomGenerator.CurrentRoomConfig.Entrance) && i.SpawnWhenExitIs.HasFlag(Commons.RoomGenerator.CurrentRoomConfig.Exit))
            .ToArray();
        
        //If there are no candidates, then choose to destroy self or not
        if (availableCandidates.Length == 0)
        {
            if (random.NextDouble() <= ChanceToSpawnNothing)
                Destroy(gameObject);

            return;
        }


        //No accessible candidates found in group
        if (availableCandidates.All(i => i.SpawnChance == 0f))
            throw new System.InvalidOperationException($"OptionalSpawnGroup has no accessible candidates");

        //Choose a random object to spawn from the group
        OptionalObjectCandidate currentCandidate;

        do
        {
            currentCandidate = availableCandidates[random.Next(availableCandidates.Length)];
        } while (random.NextDouble() > currentCandidate.SpawnChance);

        //Disable all other objects
        foreach (var i in allCandidates)
        {
            if (i == currentCandidate)
                continue;

            i.gameObject.SetActive(false);
        }
    }
}
