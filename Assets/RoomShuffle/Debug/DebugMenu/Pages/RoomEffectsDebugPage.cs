using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * NOTE:
 * Not all room effects work as intended when loaded with this menu
 */

/// <summary>
/// Allows the developer to enable or disable room effects on the current room
/// </summary>
public class RoomEffectsDebugPage : DebugPage
{
    public override string Header { get; } = "Room Effects";

    protected override void RunItems(DebugMenu caller)
    {
        //Cycle each room effect
        foreach (RoomEffects i in typeof(RoomEffects).GetEnumValues())
        {
            //... except None
            if (i == RoomEffects.None)
                continue;

            //Create a toggle for the effect
            if (Toggle(i.ToString(), Commons.CurrentRoomEffects.HasFlag(i)))
            {
                //If the effect is active, disable it
                if (Commons.CurrentRoomEffects.HasFlag(i))
                    Commons.RoomGenerator.CurrentRoomConfig.Effect &= ~i;

                //If the effect is inactive, enable it
                else if (!Commons.CurrentRoomEffects.HasFlag(i))
                    Commons.RoomGenerator.CurrentRoomConfig.Effect |= i;

                //Reinitialize room effects
                Commons.RoomEffectController.OnRoomStart(Commons.RoomGenerator.CurrentRoomConfig);
            }
        }
    }
}