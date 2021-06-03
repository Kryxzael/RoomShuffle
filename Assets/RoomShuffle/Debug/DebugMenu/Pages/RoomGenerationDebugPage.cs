using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// The room generation debug page
/// </summary>
public class RoomGenerationDebugPage : DebugPage
{
    public override string Header { get; } = "Room Generation";

    protected override void RunItems(DebugMenu caller)
    {
        if (Commons.RoomGenerator == null)
        {
            //Exit if there is no generator to manipulate
            caller.NavigationStack.Pop();
            return;
        }       

        if (Button("Regenerate"))
            Commons.RoomGenerator.GenerateNext();

        if (Button("Load Level ..."))
            caller.NavigationStack.Push(new LoadLevelDebugPage());

        if (Button("Respawn"))
            Commons.RespawnPlayer();

        if (Commons.RoomGenerator.CurrentRoomConfig != null)
        {
            //Room Theme toggle (NOTE: This does not change the background or lighting settings)
            if (Button("Room Theme [" + Commons.RoomGenerator.CurrentRoomConfig.Theme + "]"))
            {
                //Cycle the room theme
                int max = System.Enum.GetValues(typeof(RoomTheme)).Length;
                int current = (int)Commons.RoomGenerator.CurrentRoomConfig.Theme;

                Commons.RoomGenerator.CurrentRoomConfig.Theme = (RoomTheme)((current + 1) % max);

                Commons.RoomGenerator.ReloadTheme(Commons.RoomGenerator.CurrentRoomConfig.Theme, Commons.RoomGenerator.CurrentRoomConfig.Effect.HasFlag(RoomEffects.Darkness));
            }

            if (Button("Room Effects"))
                caller.NavigationStack.Push(new RoomEffectsDebugPage());
        }
    }
}
