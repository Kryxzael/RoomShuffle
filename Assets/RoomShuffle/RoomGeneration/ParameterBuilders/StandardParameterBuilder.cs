using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[CreateAssetMenu(menuName = "Room Parameter Builders/Standard")]
public class StandardParameterBuilder : ParameterBuilder
{
    public int TransitionFrequency = 5;

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
