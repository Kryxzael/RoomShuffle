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
    public int TransitionFrequency = 5;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="history"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public override RoomParameters GetNextParameters(RoomHistory history, System.Random random)
    {
        RoomParameters output = new RoomParameters();

        if (history.RoomsSinceClass(RoomClass.Transition) >= TransitionFrequency)
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
}
