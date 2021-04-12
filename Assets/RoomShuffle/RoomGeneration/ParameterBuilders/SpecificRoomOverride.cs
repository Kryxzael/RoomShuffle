using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Overrides the parameter builder to return a specific room layout
/// </summary>
public class SpecificRoomOverride : ParameterBuilderOverride
{
    /// <summary>
    /// The room to override to
    /// </summary>
    public RoomLayout Room { get; private set; }

    /// <summary>
    /// The original builder of the room generator to use this object at
    /// </summary>
    public ParameterBuilder OriginalBuilder;

    /// <summary>
    /// Initializes the room override
    /// </summary>
    /// <param name="room"></param>
    /// <param name="original"></param>
    public void Init(RoomLayout room, ParameterBuilder original)
    {
        Room = room;
        OriginalBuilder = original;
    }

    public override RoomParameters GetInitialParameters(Random random)
    {
        var original = OriginalBuilder.GetInitialParameters(random);
        original.Layout = Room;

        Room = null;
        return original;
    }

    public override RoomParameters GetNextParameters(RoomHistory history, Random random)
    {
        var original = OriginalBuilder.GetNextParameters(history, random);
        original.Layout = Room;

        Room = null;
        return original;
    }

    public override bool HasNext()
    {
        return Room != null;
    }
}