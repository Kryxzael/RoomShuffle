using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Allows the developer to enable or disable room effects on the current room
/// </summary>
public class RoomEffectsDebugPage : DebugPage
{
    public override string Header { get; } = "Room Effects";

    protected override void RunItems(DebugMenu caller)
    {
        foreach (RoomEffects i in typeof(RoomEffects).GetEnumValues())
        {
            if (i == RoomEffects.None)
                continue;

            if (Toggle(i.ToString(), Commons.CurrentRoomEffects.HasFlag(i)))
            {
                if (Commons.CurrentRoomEffects.HasFlag(i))
                    Commons.RoomGenerator.CurrentRoomConfig.Effect &= ~i;

                else if (!Commons.CurrentRoomEffects.HasFlag(i))
                    Commons.RoomGenerator.CurrentRoomConfig.Effect |= i;

                Commons.RoomEffectController.OnRoomStart(Commons.RoomGenerator.CurrentRoomConfig);
            }
        }
    }
}