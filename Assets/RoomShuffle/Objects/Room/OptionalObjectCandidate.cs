using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Allows an object in childed to an OptionalObjectGroup to be randomly spawned by the room generator
/// </summary>
public class OptionalObjectCandidate : MonoBehaviour
{
    [Tooltip("The chance the generator has to pick this object")]
    [Range(0f, 1f)]
    public float SpawnChance;

    private void Start()
    {
        if (transform.parent == null || transform.parent.GetComponent<OptionalObjectGroup>() == null)
            throw new InvalidOperationException("OptionalObjectCandidates can only appear as direct children of OptionalObjectGroups");
    }
}