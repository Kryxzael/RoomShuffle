using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

using SysRandom = System.Random;

/// <summary>
/// Game manager that controls room generation
/// </summary>
public class RoomGenerator : MonoBehaviour
{
    [Tooltip("Will the generator automatically generate the first room when the it starts?")]
    public bool AutoGenerateFirstRoom = true;

    [Header("Seeding")]
    [Tooltip("The set seed the generator will use if 'Use Random Seed' is disabled")]
    public int Seed;

    [Tooltip("Whether the generator should set its seed randomly. If this is enabled, the 'Seed' property is ignored")]
    public bool UseRandomSeed;

    [Header("Generators")]
    [Tooltip("The room parameter picker the generator will use")]
    public ParameterBuilder RoomParameterBuilder;

    /// <summary>
    /// Gets the room parameter builders that are currently being used to override the primary room parameter builder
    /// </summary>
    public Stack<ParameterBuilderOverride> RoomParameterBuilderOverrides { get; } = new Stack<ParameterBuilderOverride>();

    /* *** */

    /// <summary>
    /// The random number generator used to generate rooms
    /// </summary>
    private SysRandom RoomRng;

    /*
     * Generation History
     */

    /// <summary>
    /// The root object of the currently generated room
    /// </summary>
    private GameObject CurrentRoomObject;

    /// <summary>
    /// Gets the current room's parameter
    /// </summary>
    public RoomParameters CurrentRoomConfig
    {
        get
        {
            return History.FirstOrDefault();
        }
    }

    /// <summary>
    /// The history of the room generator
    /// </summary>
    public RoomHistory History = new RoomHistory();
 

    private void Awake()
    {
        if (UseRandomSeed)
            Seed = Random.Range(0, int.MaxValue);

        RoomRng = new SysRandom(Seed);
    }

    private void Start()
    {
        if (AutoGenerateFirstRoom)
            GenerateNext();
    }

    /// <summary>
    /// Generates the next room
    /// </summary>
    public void GenerateNext()
    {
        //Destroy current room
        if (CurrentRoomObject != null)
            Destroy(CurrentRoomObject);

        /*
         * Create the room configuration
         */
        RoomParameters parameters;
        var builder = RoomParameterBuilder;

        while (RoomParameterBuilderOverrides.Any())
        {
            if (RoomParameterBuilderOverrides.Peek().HasNext())
            {
                builder = RoomParameterBuilderOverrides.Peek();
                break;
            }

            RoomParameterBuilderOverrides.Pop();
        }

        if (CurrentRoomConfig == null)
            parameters = builder.GetInitialParameters(RoomRng);
        else
            parameters = builder.GetNextParameters(History, RoomRng);

        CurrentRoomObject = Instantiate(parameters.Layout).gameObject;

        //Create generation entry
        GeneratedRoom generatedRoom = CurrentRoomObject.AddComponent<GeneratedRoom>();
        generatedRoom.Parameters = parameters;

        //Add to history
        History.RegisterHistory(parameters);

        DebugScreenDrawer.Enable("roomcount", "room: " + History.Count());

        //Reload tilesets
        foreach (Tilemap i in FindObjectsOfType<Tilemap>())
            i.RefreshAllTiles();

        //Clear puzzle keys
        Commons.Inventory.PuzzleKeys = 0;

        /*
         * Set optional objects
         */
        //Go over every spawn group
        foreach (var i in CurrentRoomObject.GetComponentsInChildren<OptionalObjectGroup>())
            i.Select(RoomRng);

        //Spawn treasure
        foreach (var i in CurrentRoomObject.GetComponentsInChildren<RandomItem>())
            i.SpawnAndDestroy(RoomRng);

        //Set up room effects
        Commons.RoomEffectController.OnRoomStart(parameters);

        //Flip camera
        FlipCamera.IsFlipped = CurrentRoomConfig.FlipHorizontal;
    }
}
