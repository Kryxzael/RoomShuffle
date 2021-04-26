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
    public RoomLayout TransitionLeftToTop;
    public RoomLayout TransitionLeftToBottom;

    [Space]
    public RoomLayout TransitionRightToTop;
    public RoomLayout TransitionRightToBottom;

    [Space]
    public RoomLayout TransitionTopToLeft;
    public RoomLayout TransitionTopToRight;

    [Space]
    public RoomLayout TransitionBottomToLeft;
    public RoomLayout TransitionBottomToRight;

    [Space]
    public RoomLayout TransitionBottomToBottom;
    public RoomLayout TransitionTopToTop;
    public RoomLayout TransitionLeftToLeft;
    public RoomLayout TransitionRightToRight;

    public List<RoomLayout> RespiteRooms = new List<RoomLayout>();

    [Header("== SPECIAL ROOMS ==")]
    public List<RoomLayout> StartingRooms = new List<RoomLayout>();
    public List<RoomLayout> ShopRooms = new List<RoomLayout>();
    public List<RoomLayout> SecretRooms = new List<RoomLayout>();

    /// <summary>
    /// Gets the transition room that would fit between a room with the provided exit side and another room with the provided entrance side
    /// </summary>
    /// <param name="preTransitionExit"></param>
    /// <param name="postTransitionEntrance"></param>
    /// <returns></returns>
    public RoomLayout GetTransitionRoomByDirections(EntranceExitSides preTransitionExit, EntranceExitSides postTransitionEntrance)
    {
        if (preTransitionExit == EntranceExitSides.Bottom)
        {
            return postTransitionEntrance switch
            {
                EntranceExitSides.Bottom => TransitionTopToTop,
                EntranceExitSides.Right => TransitionTopToLeft,
                EntranceExitSides.Left => TransitionTopToRight,
                _ => null
            };
        }

        else if (preTransitionExit == EntranceExitSides.Top)
        {
            return postTransitionEntrance switch
            {
                EntranceExitSides.Top => TransitionBottomToBottom,
                EntranceExitSides.Right => TransitionBottomToLeft,
                EntranceExitSides.Left => TransitionBottomToRight,
                _ => null
            };
        }

        else if (preTransitionExit == EntranceExitSides.Left)
        {
            return postTransitionEntrance switch
            {
                EntranceExitSides.Left => TransitionRightToRight,
                EntranceExitSides.Top => TransitionRightToBottom,
                EntranceExitSides.Bottom => TransitionRightToTop,
                _ => null
            };
        }

        else if (preTransitionExit == EntranceExitSides.Right)
        {
            return postTransitionEntrance switch
            {
                EntranceExitSides.Right => TransitionLeftToLeft,
                EntranceExitSides.Top => TransitionLeftToBottom,
                EntranceExitSides.Bottom => TransitionLeftToTop,
                _ => null
            };
        }

        return null;
    }
}
