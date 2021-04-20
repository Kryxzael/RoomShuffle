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

    [Header("Effects")]
    [Tooltip("What room effects to exclude when picking an effect for this room")]
    public RoomEffects ExcludedEffects = RoomEffects.None;

    [Tooltip("The amount of time the player will have to complete the room when the timer effect is active. Ignored if timer effect is excluded")]
    public float TimerEffectSeconds = 300f;

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
    public EntranceExitSides GetRandomExit(System.Random random, EntranceExitSides excludedEntranceSide)
    {
        if (ExitSides == excludedEntranceSide)
            throw new InvalidOperationException($"This room ({gameObject.name}) does not have any exits that are not on the entrance side");

        EntranceExitSides output;

        do
        {
            output = GetRandomSide(ExitSides, random);
        } while (output == excludedEntranceSide);

        return output;
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
