using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

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
}
