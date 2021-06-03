using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Changes the room theme when the button is pressed
/// </summary>
public class ChangeRoomThemeButton : Button
{
    [Tooltip("The room theme to change to")]
    public RoomTheme Theme;

    public override void Press()
    {
        base.Press();

        var roomGenerator = Commons.RoomGenerator;

        if (roomGenerator.CurrentRoomConfig.Theme == Theme)
            return;

        roomGenerator.CurrentRoomConfig.Theme = Theme;
        roomGenerator.ReloadTheme(roomGenerator.CurrentRoomConfig.Theme, roomGenerator.CurrentRoomConfig.Effect.HasFlag(RoomEffects.Darkness));

    }
}
