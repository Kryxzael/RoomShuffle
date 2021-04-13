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
    [Tooltip("How many rooms can be generated before a transition room will be generated")]
    public RandomValueBetween TransitionFrequency = new RandomValueBetween(5, 8);

    /// <summary>
    /// How often the room theme should change
    /// </summary>
    public RandomValueBetween ThemeChangeFrequency = new RandomValueBetween(3, 10);

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="history"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public override RoomParameters GetNextParameters(RoomHistory history, System.Random random)
    {
        RoomParameters output = new RoomParameters();
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
         * Room Class
         */

        //Pick a transition room
        if (history.RoomsSinceClass(RoomClass.Transition) >= TransitionFrequency.PickInt(random))
        {
            output.Class = RoomClass.Transition;
        }

        //Pick a platforming room
        else
        {
            output.Class = RoomClass.Platforming;
        }

        //Pick random room-layout and flip-state
        (output.Layout, output.FlipHorizontal, output.Entrance) = Rooms.PickRandomToMatchPreviousExit(output.Class, history.First().Exit, random);

        //Pick random exit
        output.Exit = output.Layout.GetRandomExit(random);
        

        return output;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public override RoomParameters GetInitialParameters(System.Random random)
    {
        var layout = Rooms.PlatformingRooms[random.Next(Rooms.PlatformingRooms.Count)];

        return new RoomParameters
        {
            EnemySet = EnemySets[random.Next(EnemySets.Count)],
            Theme = (RoomTheme)random.Next(1, typeof(RoomTheme).GetEnumValues().Length),
            Class = RoomClass.Platforming,
            Layout = layout,
            Entrance = layout.GetRandomEntrance(random),
            Exit = layout.GetRandomExit(random)
        };
    }
}
