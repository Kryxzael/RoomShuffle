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

    private RoomLayout _queuedLayout;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="history"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public override RoomParameters GetNextParameters(RoomHistory history, System.Random random)
    {
        RoomParameters output = new RoomParameters();
        RoomLayout nextLayout;

        output.EnemySet = EnemySets[random.Next(EnemySets.Count)];

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
         * Room Class and Queued Layout
         */

        //Time for a shop
        if (history.RoomsSinceClass(RoomClass.Shop) >= ShopFrequency.Pick())
        {
            output.Class = RoomClass.Shop;
            nextLayout = Rooms.ShopRooms[random.Next(Rooms.ShopRooms.Count)];
        }

        //Time for a puzzle
        else if (history.RoomsSinceClass(RoomClass.Puzzle) >= PuzzleFrequency.Pick())
        {
            output.Class = RoomClass.Puzzle;
            nextLayout = NextLayout(ref _puzzleLayoutEnumerator, Rooms.PuzzleRooms, random);
        }

        //Time for a eradication room
        else if (history.RoomsSinceClass(RoomClass.Eradication) >= EradicationFrequency.Pick())
        {
            output.Class = RoomClass.Eradication;
            nextLayout = NextLayout(ref _eradicationLayoutEnumerator, Rooms.EradicationRooms, random);
        }

        //Pick a platforming room
        else
        {
            output.Class = RoomClass.Platforming;
            nextLayout = NextLayout(ref _platformLayoutEnumerator, Rooms.PlatformingRooms, random);
        }

        /*
         * Room effects
         */

        if (history.RoomsSinceMatchOfPredicate(i => i.Effect != RoomEffects.None) >= RoomEffectFrequency.Pick())
        {
            output.Effect = typeof(RoomEffects)
                .GetEnumValues()
                .Cast<RoomEffects>() 
                .Except(new[] { RoomEffects.None })
                .Where(i => !output.Layout.ExcludedEffects.HasFlag(i))
                .OrderBy(i => random.Next())
                .FirstOrDefault();
        }

        /*
         * Weapon enumerator
         */
        output.WeaponEnumerator = WeaponTemplates
            .OrderBy(i => random.Next())
            .GetEnumerator();
            
        
        /*
         * Glue rooms together
         */

        //Something's in the queue, consume it
        if (_queuedLayout != null)
        {
            nextLayout = _queuedLayout;
            _queuedLayout = null;
        }

        //Choose entrance direction, flip state and potentially create a transition room between the last and the queued room
        (output.Layout, output.FlipHorizontal, output.Entrance, output.Exit) = Rooms.CreateTransition(history.First(), nextLayout, random);

        //If the output's layout is different from what the previous function returned, then a transition has been made
        //Queue the next layout to have it be generated next
        if (output.Layout != nextLayout)
        {
            output.Class = RoomClass.Transition;
            _queuedLayout = nextLayout;
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
            EnemySet = EnemySets[random.Next(EnemySets.Count)],
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
