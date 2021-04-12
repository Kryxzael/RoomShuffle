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
        foreach (var i in Resources.FindObjectsOfTypeAll<RoomLayout>().OrderBy(i => i.name))
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
