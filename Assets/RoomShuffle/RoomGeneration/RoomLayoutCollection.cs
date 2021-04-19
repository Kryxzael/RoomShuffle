using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Collection containing and categorizing room layouts
/// </summary>
[CreateAssetMenu(menuName = "Room Layout Collection")]
public class RoomLayoutCollection : ScriptableObject
{
    [Header("== STANDARD ROOMS ==")]
    public List<RoomLayout> PlatformingRooms = new List<RoomLayout>();
    public List<RoomLayout> PuzzleRooms = new List<RoomLayout>();
    public List<RoomLayout> EradicationRooms = new List<RoomLayout>();
    public List<RoomLayout> BossRooms = new List<RoomLayout>();

    [Header("== TRANSITIONAL ROOMS ==")]
    public List<RoomLayout> TransitionRooms = new List<RoomLayout>();
    public List<RoomLayout> CrossroadRooms = new List<RoomLayout>();
    public List<RoomLayout> RespiteRooms = new List<RoomLayout>();

    [Header("== SPECIAL ROOMS ==")]
    public List<RoomLayout> StartingRooms = new List<RoomLayout>();
    public List<RoomLayout> ShopRooms = new List<RoomLayout>();
    public List<RoomLayout> SecretRooms = new List<RoomLayout>();

    /// <summary>
    /// Gets a transition room between the last and the next room
    /// </summary>
    /// <param name="last"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public (RoomLayout layout, bool shouldFlip, EntranceExitSides entranceSide, EntranceExitSides exitSide) CreateTransition(RoomParameters last, RoomLayout next, System.Random random)
    {
        //Start with the next room, to see if no transition room is needed
        var candidate = next;

        //Choose the entrance position based on the last exit position
        var entrance = last.Exit switch
        {
            EntranceExitSides.Top => EntranceExitSides.Bottom,
            EntranceExitSides.Right => EntranceExitSides.Left,
            EntranceExitSides.Bottom => EntranceExitSides.Top,
            EntranceExitSides.Left => EntranceExitSides.Left, //Right-entranced rooms are just flipped
            _ => throw new InvalidOperationException()
        };

        //Choose if the room is flipped
        bool shouldFlip = last.Exit switch
        {
            EntranceExitSides.Top => random.Next(2) == 0,
            EntranceExitSides.Right => last.FlipHorizontal,
            EntranceExitSides.Bottom => random.Next(2) == 0,
            EntranceExitSides.Left => !last.FlipHorizontal,
            _ => throw new InvalidOperationException()
        };

        //Choose the exit position
        //var exit = next.GetRandomExit(random, entrance);
        var exit = next.GetRandomExit(random, EntranceExitSides.None);

        //If the current candidate does not have the provided entrance or exit, choose a new candidate (transition room)
        while (!candidate.EntranceSides.HasFlag(entrance) || !candidate.ExitSides.HasFlag(exit))
        {
            //Choose a random transition room
            candidate = TransitionRooms[random.Next(TransitionRooms.Count)];

            //Choose a new exit based on the next room's available exits
            //Note: This call makes it so that multiple transitions may spawn after each other and could probably be optimized
            exit = candidate.GetRandomExit(random, entrance);
        }

        return (candidate, shouldFlip, entrance, exit);
    }
}
