using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SysRandom = System.Random;

/// <summary>
/// Game manager that controls room generation
/// </summary>
public class RoomGenerator : MonoBehaviour
{
    [Header("Seeding")]
    public int Seed;
    public bool UseRandomSeed;

    [Header("Rooms")]
    public List<RoomLayout> Layouts = new List<RoomLayout>();

    /* *** */

    private SysRandom RoomRng;
    private RoomLayout CurrentRoom;

    private void Awake()
    {
        if (!UseRandomSeed)
            Seed = Random.Range(0, int.MaxValue);

        RoomRng = new SysRandom(Seed);
    }

    private void Start()
    {
        GenerateNext();
    }

    /// <summary>
    /// Generates the next room
    /// </summary>
    public void GenerateNext()
    {
        if (CurrentRoom != null)
            Destroy(CurrentRoom.gameObject);

        RoomLayout nextRoom = GetNextRoom();
        CurrentRoom = Instantiate(nextRoom);
    }

    private RoomLayout GetNextRoom()
    {
        return Layouts[RoomRng.Next(Layouts.Count)];
    }
}
