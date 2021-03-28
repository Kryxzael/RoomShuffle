using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Makes an object be randomly toggleable by the room generator
/// </summary>
public class OptionalRoomObject : MonoBehaviour
{
    [Tooltip("The chance the object has of being present")]
    [Range(0f, 1f)]
    public float SpawnChance = 1f;

    [Tooltip("The spawn group the object is part of. Only one object in any given spawn group is selected")]
    public int SpawnGroup;
}
