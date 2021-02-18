using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Defines a room layout and its settings. Placed on the root object of a room prefab
/// </summary>
public class RoomLayout : MonoBehaviour
{
    [Header("Entrances and Exits")]
    [Tooltip("The side of the room where the entrance is")]
    public Direction4 EntranceSide;

    [Tooltip("The side of the room where the exit is")]
    public Direction4 ExitSide;
}
