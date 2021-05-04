using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Shows a list of level classes that levels can be loaded from
/// </summary>
public class LoadLevelDebugPage : DebugPage
{
    public override string Header { get; } = "Load Level";

    protected override void RunItems(DebugMenu caller)
    {
        if (Button("Platformers ... "))
            caller.NavigationStack.Push(new LoadLevelFromListDebugPage("Platformers", Commons.RoomGenerator.RoomParameterBuilder.Rooms.PlatformingRooms));

        if (Button("Puzzles ... "))
            caller.NavigationStack.Push(new LoadLevelFromListDebugPage("Puzzles", Commons.RoomGenerator.RoomParameterBuilder.Rooms.PuzzleRooms));

        if (Button("Eradications ... "))
            caller.NavigationStack.Push(new LoadLevelFromListDebugPage("Eradications", Commons.RoomGenerator.RoomParameterBuilder.Rooms.EradicationRooms));

        if (Button("Shops ... "))
            caller.NavigationStack.Push(new LoadLevelFromListDebugPage("Shops", Commons.RoomGenerator.RoomParameterBuilder.Rooms.ShopRooms));

        if (Button("Respite ... "))
            caller.NavigationStack.Push(new LoadLevelFromListDebugPage("Respite", Commons.RoomGenerator.RoomParameterBuilder.Rooms.RespiteRooms));

        if (Button("Openings ... "))
            caller.NavigationStack.Push(new LoadLevelFromListDebugPage("Openings", Commons.RoomGenerator.RoomParameterBuilder.Rooms.StartingRooms));
    }
}

/// <summary>
/// Lets the player load a specific level from a list of rooms
/// </summary>
public class LoadLevelFromListDebugPage : DebugPage
{
    public override string Header { get; }
    private IEnumerable<RoomLayout> Layouts { get; }

    public LoadLevelFromListDebugPage(string header, IEnumerable<RoomLayout> layouts)
    {
        Header = header;
        Layouts = layouts.OrderBy(i => i.name);
    }

    protected override void RunItems(DebugMenu caller)
    {
        //Create a button for each room
        foreach (var i in Layouts)
        {
            if (Button(i.name))
            {
                //When a room is selected. Create a SpecificRoomOverride with the room and push it to the generator
                var roomOverride = ScriptableObject.CreateInstance<SpecificRoomOverride>();
                roomOverride.Init(i, Commons.RoomGenerator.RoomParameterBuilder);

                Commons.RoomGenerator.RoomParameterBuilderOverrides.Push(roomOverride);
                Commons.RoomGenerator.GenerateNext();
            }

        }
    }
}
