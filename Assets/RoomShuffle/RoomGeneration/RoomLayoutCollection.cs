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
    public List<RoomLayout> ShopRooms = new List<RoomLayout>();
    public List<RoomLayout> SecretRooms = new List<RoomLayout>();

    /// <summary>
    /// Picks a random room matching with the direction of the previous room's exit and a given room class
    /// </summary>
    /// <param name="class"></param>
    /// <param name="lastRoomExit"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public (RoomLayout room, bool shouldFlip, EntranceExitSides entraceSide) PickRandomToMatchPreviousExit(RoomClass @class, EntranceExitSides lastRoomExit, System.Random random)
    {
        //Calculate which list to choose from
        List<RoomLayout> rooms = @class switch
        {
            RoomClass.Platforming => PlatformingRooms,
            RoomClass.Respite => RespiteRooms,
            RoomClass.Transition => TransitionRooms,
            RoomClass.Eradication => EradicationRooms,
            RoomClass.Puzzle => PuzzleRooms,
            RoomClass.Secret => SecretRooms,
            RoomClass.Crossroads => CrossroadRooms,
            RoomClass.Boss => BossRooms,

            _ => throw new ArgumentException(nameof(@class)),
        };

        //Select the next entrance side
        EntranceExitSides entraceSide = lastRoomExit switch
        {
            //If last exit was on top or bottom, the new room must have an entrance at the opposite side
            EntranceExitSides.Top => EntranceExitSides.Bottom,
            EntranceExitSides.Bottom => EntranceExitSides.Top,

            //If last exit was on either side of the room, the new room must have an entrance at the left side (which will later be flipped if neccesary)
            _ => EntranceExitSides.Left
        };

        //Then calculate whether the room should be flipped
        bool shouldFlip = lastRoomExit switch
        {
            //If the last room exited vertically, leave it to chance
            EntranceExitSides.Top => random.Next(2) == 0,
            EntranceExitSides.Bottom => random.Next(2) == 0,

            //If the last room exited on the left, the room must be flipped
            EntranceExitSides.Left => true,

            //If the last room exited on the right, the room must be not flipped
            _ => false
        };

        //Further filter to rooms with entrance on the correct side
        rooms = rooms.Where(i => i.EntranceSides.HasFlag(entraceSide)).ToList();

        //Finally, pick a room and return the result
        return (rooms[random.Next(rooms.Count)], shouldFlip, entraceSide);
    }
}
