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
    [Tooltip("The side(s) of the room where the entrance(s) is/are")]
    public EntranceExitSides EntranceSides;

    [Tooltip("The side(s) of the room where the exit(s) is/are")]
    public EntranceExitSides ExitSides;

    /// <summary>
    /// Gets a random entrance direction for this room based on its exit sides
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public EntranceExitSides GetRandomEntrance(System.Random random)
    {
        return GetRandomSide(EntranceSides, random);
    }

    /// <summary>
    /// Gets a random exit direction for this room based on its exit sides
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public EntranceExitSides GetRandomExit(System.Random random)
    {
        return GetRandomSide(ExitSides, random);
    }

    private EntranceExitSides GetRandomSide(EntranceExitSides flags, System.Random random)
    {
        //Select exit location
        foreach (EntranceExitSides i in typeof(EntranceExitSides).GetEnumValues().Cast<EntranceExitSides>().OrderBy(i => random.Next()))
        {
            if (i == EntranceExitSides.None)
                continue;

            if (flags.HasFlag(i))
                return i;
        }

        throw new InvalidOperationException("This room has no exits");
    }
}

[Flags]
public enum EntranceExitSides
{
    None = 0x0,
    Top = 0x1,
    Right = 0x2,
    Bottom = 0x4,
    Left = 0x8,
}
