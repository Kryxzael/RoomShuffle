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
    public Direction4 EntranceSide;
    public Direction4 ExitSide;
}
