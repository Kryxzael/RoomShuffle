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

        if (history.RoomsSinceThemeChange() >= ThemeChangeFrequency.PickInt())
        {
            do
            {
                output.Theme = (RoomTheme)random.Next(1, typeof(RoomTheme).GetEnumValues().Length);
            } while (output.Theme == history.First().Theme);
        }
        else
        {
            output.Theme = history.First().Theme;
        }


        if (history.RoomsSinceClass(RoomClass.Transition) >= TransitionFrequency.PickInt(random))
        {
            output.Class = RoomClass.Transition;
            output.Layout = Rooms.TransitionRooms[random.Next(Rooms.TransitionRooms.Count)];
        }
        else
        {
            output.Class = RoomClass.Platforming;
            output.Layout = Rooms.PlatformingRooms[random.Next(Rooms.PlatformingRooms.Count)];
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
        return new RoomParameters
        {
            Theme = (RoomTheme)random.Next(1, typeof(RoomTheme).GetEnumValues().Length),
            Class = RoomClass.Platforming,
            Layout = Rooms.PlatformingRooms[random.Next(Rooms.PlatformingRooms.Count)]
        };
    }
}
