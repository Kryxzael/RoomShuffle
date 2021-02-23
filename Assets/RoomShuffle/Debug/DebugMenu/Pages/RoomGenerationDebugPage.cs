using TMPro.EditorUtilities;

using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// The room generation debug page
/// </summary>
public class RoomGenerationDebugPage : DebugPage
{
    private static RoomGenerator _bufferedRoomGenerator;

    private static RoomGenerator BufferedRoomGenerator
    {
        get
        {
            if (_bufferedRoomGenerator == null)
                _bufferedRoomGenerator = Object.FindObjectOfType<RoomGenerator>();

            return _bufferedRoomGenerator;
        }
    }

    public override string Header { get; } = "Room Generation";

    protected override void RunItems(DebugMenu caller)
    {
        if (BufferedRoomGenerator == null)
            return;

        if (Button("Regenerate"))
            BufferedRoomGenerator.GenerateNext();

        if (Button("Room Theme [" + BufferedRoomGenerator.CurrentRoomConfig.Theme + "]"))
        {
            int max = System.Enum.GetValues(typeof(RoomTheme)).Length;
            int current = (int)BufferedRoomGenerator.CurrentRoomConfig.Theme;

            BufferedRoomGenerator.CurrentRoomConfig.Theme = (RoomTheme)((current + 1) % max);

            foreach (Tilemap i in Object.FindObjectsOfType<Tilemap>())
                i.RefreshAllTiles();
        }
            
    }
}
