using NUnit.Framework;

using System;
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

    [Header("Generator")]
    [Tooltip("The room parameter picker the generator will use")]
    public ParameterBuilder RoomParameterBuilder;

    [Header("Backgrounds")]
    [Tooltip("Gets the backgrounds to use for the different room themes")]
    public List<RoomThemeBackgroundMapping> Backgrounds = new List<RoomThemeBackgroundMapping>();

    /* *** */

    /// <summary>
    /// Gets the room parameter builders that are currently being used to override the primary room parameter builder
    /// </summary>
    public Stack<ParameterBuilderOverride> RoomParameterBuilderOverrides { get; } = new Stack<ParameterBuilderOverride>();

    /// <summary>
    /// Gets or sets the number that is displayed as the room count
    /// </summary>
    public int CurrentRoomNumber { get; set; }

    /// <summary>
    /// The random number generator used to generate rooms
    /// </summary>
    public SysRandom RoomRng { get; private set; }

    /*
     * Generation History
     */

    /// <summary>
    /// The root object of the currently generated room
    /// </summary>
    private GameObject CurrentRoomObject;

    /// <summary>
    /// The background object of the currently generated room
    /// </summary>
    private GameObject CurrentBackgroundObject;

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
            Seed = UnityEngine.Random.Range(0, int.MaxValue);

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
        /*
         * Destroy current room and background
         */
        DestroyImmediate(CurrentRoomObject);
        Destroy(CurrentBackgroundObject);


        /*
         * Create the room configuration
         */
        RoomParameters parameters;
        var builder = RoomParameterBuilder;

        //Use parameter builder overrides if present
        while (RoomParameterBuilderOverrides.Any())
        {
            if (RoomParameterBuilderOverrides.Peek().HasNext())
            {
                builder = RoomParameterBuilderOverrides.Peek();
                break;
            }

            RoomParameterBuilderOverrides.Pop();
        }

        //Generate parameters with builder
        if (CurrentRoomConfig == null)
            parameters = builder.GetInitialParameters(RoomRng);
        else
            parameters = builder.GetNextParameters(History, RoomRng);

        /*
         * Spawn room
         */
        CurrentRoomObject = Instantiate(parameters.Layout).gameObject;

        //Create generation entry
        GeneratedRoom generatedRoom = CurrentRoomObject.AddComponent<GeneratedRoom>();
        generatedRoom.Parameters = parameters;

        //Add to history
        History.RegisterHistory(parameters);

        /*
         * Spawn background
         */
        if (!parameters.Effect.HasFlag(RoomEffects.Darkness))
        {
            var background = Backgrounds.SingleOrDefault(i => i.Theme == parameters.Theme);
            
            if (background != null)
                CurrentBackgroundObject = Instantiate(background.Background);
        }

        /*
         * Post-generation setup
         */

        //Reload tilesets
        foreach (Tilemap i in FindObjectsOfType<Tilemap>())
            i.RefreshAllTiles();

        //Clear puzzle keys
        Commons.Inventory.PuzzleKeys = 0;

        //Set optional objects
        foreach (var i in CurrentRoomObject.GetComponentsInChildren<OptionalObjectGroup>())
            i.Select(RoomRng);

        //Set up room effects
        Commons.RoomEffectController.OnRoomStart(parameters);

        //Flip camera
        FlipCamera.IsFlipped = CurrentRoomConfig.FlipHorizontal;

        //Increase room number
        if (!parameters.Class.IsSafeRoom())
            CurrentRoomNumber++;

        //Spawn the player
        FindObjectOfType<Entrance>().SpawnPlayer();
    }


    [Serializable]
    public class RoomThemeBackgroundMapping
    {
        public RoomTheme Theme;
        public GameObject Background;
    }
}
