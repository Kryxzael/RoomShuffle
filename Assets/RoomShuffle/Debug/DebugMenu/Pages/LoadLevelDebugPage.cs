using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

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
        foreach (var i in Layouts)
        {
            if (Button(i.name))
            {
                var roomOverride = ScriptableObject.CreateInstance<SpecificRoomOverride>();
                roomOverride.Init(i, Commons.RoomGenerator.RoomParameterBuilder);

                Commons.RoomGenerator.RoomParameterBuilderOverrides.Push(roomOverride);
                Commons.RoomGenerator.GenerateNext();
            }

        }
    }
}
