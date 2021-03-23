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

        if (Commons.RoomGenerator.CurrentRoomConfig != null)
        {
            if (Button("Room Theme [" + Commons.RoomGenerator.CurrentRoomConfig.Theme + "]"))
            {
                int max = System.Enum.GetValues(typeof(RoomTheme)).Length;
                int current = (int)Commons.RoomGenerator.CurrentRoomConfig.Theme;

                Commons.RoomGenerator.CurrentRoomConfig.Theme = (RoomTheme)((current + 1) % max);

                foreach (Tilemap i in Object.FindObjectsOfType<Tilemap>())
                    i.RefreshAllTiles();
            }

            if (Button("Room Effects"))
                caller.NavigationStack.Push(new RoomEffectsDebugPage());
        }
    }
}
