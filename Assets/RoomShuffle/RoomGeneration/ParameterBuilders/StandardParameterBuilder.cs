using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// The default room parameter room builder
/// </summary>
[CreateAssetMenu(menuName = "Room Parameter Builders/Standard")]
public class StandardParameterBuilder : ParameterBuilder
{
    [Header("Room Class Frequency")]
    [Tooltip("How many rooms can be generated before a transition room will be generated")]
    public RandomValueBetween ShopFrequency = (5, 10);

    [Tooltip("How many rooms can be generated before a transition room will be generated")]
    public RandomValueBetween PuzzleFrequency = (4, 7);

    [Tooltip("How many rooms can be generated before a transition room will be generated")]
    public RandomValueBetween EradicationFrequency = (9, 13);

    [Header("General Frequency")]
    [Tooltip("How often the room theme should change")]
    public RandomValueBetween ThemeChangeFrequency = (3, 10);

    [Tooltip("How often a room effect should be applied")]
    public RandomValueBetween RoomEffectFrequency = (4, 6);

    /* *** */

    /*
     * Enumerators used to predetermine the level layouts to use
     */

    private IEnumerator<RoomLayout> _platformLayoutEnumerator;
    private IEnumerator<RoomLayout> _puzzleLayoutEnumerator;
    private IEnumerator<RoomLayout> _eradicationLayoutEnumerator;

    [NonSerialized]
    private RoomParameters _queued;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="history"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public override RoomParameters GetNextParameters(RoomHistory history, System.Random random)
    {
        RoomParameters output;

        /*
         * Room Class and Queued Layout
         */

        //A room has been queued
        if (_queued != null)
        {
            output = _queued;
            _queued = null;
        }

        //No room is queued
        else
        {
            output = new RoomParameters();

            //Choose random enemy set
            output.GroundEnemies = GroundEnemies;
            output.AirEnemies = AirEnemies;

            //The player need help, slot in a respite room
            if ((RequiredItemLootTable.InNeedOfHealth() && random.Next(3) == 0) || (RequiredItemLootTable.InNeedOfWeapons() && random.Next(3) == 0) || (RequiredItemLootTable.InNeedOfMoney() && random.Next(10) == 0))
            {
                output.Class = RoomClass.Respite;
                output.Layout = Rooms.RespiteRooms[random.Next(Rooms.RespiteRooms.Count)];
            }

            //Time for a shop
            else if (history.RoomsSinceClass(RoomClass.Shop, includeSafes: true) >= ShopFrequency.Pick(random))
            {
                output.Class = RoomClass.Shop;
                output.Layout = Rooms.ShopRooms[random.Next(Rooms.ShopRooms.Count)];
            }

            //Time for a puzzle
            else if (history.RoomsSinceClass(RoomClass.Puzzle) >= PuzzleFrequency.Pick(random))
            {
                output.Class = RoomClass.Puzzle;
                output.Layout = NextLayout(ref _puzzleLayoutEnumerator, Rooms.PuzzleRooms, random);
            }

            //Time for a eradication room
            else if (history.RoomsSinceClass(RoomClass.Eradication) >= EradicationFrequency.Pick(random))
            {
                output.Class = RoomClass.Eradication;
                output.Layout = NextLayout(ref _eradicationLayoutEnumerator, Rooms.EradicationRooms, random);
            }

            //Pick a platforming room
            else
            {
                output.Class = RoomClass.Platforming;
                output.Layout = NextLayout(ref _platformLayoutEnumerator, Rooms.PlatformingRooms, random);
            }

            /*
             * Weapon enumerator
             */
            output.WeaponEnumerator = WeaponTemplates
                .OrderBy(i => random.Next())
                .GetEnumerator();

            /*
             * Flip
             */

            output.FlipHorizontal = random.Next(2) == 0;

            //Choose a random entrance and exit
            output.Entrance = output.Layout.GetRandomEntrance(random);
            output.Exit = output.Layout.GetRandomExit(random, output.Entrance);
        }

        /*
         * Check if the room requires a transition
         */
        var transitionLayout = Rooms.GetTransitionRoomByDirections(history.First().Exit, output.Entrance, history.First().FlipHorizontal, output.FlipHorizontal);

        //If it does, queue the room and use the transition instead
        if (transitionLayout)
        {
            _queued = output;
            output = new RoomParameters();

            output.Class = RoomClass.Transition;
            output.Layout = transitionLayout;
            output.FlipHorizontal = false;

            //Transition will have a new entrance-exit
            output.Entrance = transitionLayout.EntranceSides;
            output.Exit = transitionLayout.ExitSides;
        }

        /*
         * Room Theme
         */

        //Pick the next theme
        if (history.RoomsSinceThemeChange() >= ThemeChangeFrequency.PickInt())
        {
            do
            {
                output.Theme = (RoomTheme)random.Next(1, typeof(RoomTheme).GetEnumValues().Length);
            } while (output.Theme == history.First().Theme);
        }

        //Use the existing theme
        else
        {
            output.Theme = history.First().Theme;
        }

        /*
         * Room effects
         */

        if (!output.Class.IsSafeRoom() && history.RoomsSinceMatchOfPredicate(i => i.Effect != RoomEffects.None) >= RoomEffectFrequency.Pick())
        {
            output.Effect = typeof(RoomEffects)
                .GetEnumValues()
                .Cast<RoomEffects>()
                .Except(new[] { RoomEffects.None })
                .Where(i => !output.Layout.ExcludedEffects.HasFlag(i))
                .Where(i => !(Commons.SpeedRunMode && i == RoomEffects.Timer))
                .OrderBy(i => random.Next())
                .FirstOrDefault();
        }

        return output;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public override RoomParameters GetInitialParameters(System.Random random)
    {
        /*
         * Create and return the first platforming room
         */
        var layout = Rooms.StartingRooms[random.Next(Rooms.StartingRooms.Count)];

        return new RoomParameters
        {
            GroundEnemies = this.GroundEnemies,
            AirEnemies = this.AirEnemies,
            Theme = (RoomTheme)random.Next(1, typeof(RoomTheme).GetEnumValues().Length),
            Class = RoomClass.Starting,
            Layout = layout,
            Entrance = layout.GetRandomEntrance(random),
            Exit = layout.GetRandomExit(random, EntranceExitSides.None),
            WeaponEnumerator = WeaponTemplates.OrderBy(i => random.Next()).GetEnumerator()
        };
    }

    /// <summary>
    /// Gets the next room layout of the provided enumerator, and re-shuffles it if its completed
    /// </summary>
    /// <param name="enumerator"></param>
    /// <param name="source"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    private static RoomLayout NextLayout(ref IEnumerator<RoomLayout> enumerator, IEnumerable<RoomLayout> source, System.Random random)
    {
        if (enumerator == null || !enumerator.MoveNext())
        {
            enumerator = source.OrderBy(i => random.Next()).GetEnumerator();
            enumerator.MoveNext();
        }

        return enumerator.Current;
    }
}
