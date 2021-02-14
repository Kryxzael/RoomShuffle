using UnityEngine;

/// <summary>
/// The room generation debug page
/// </summary>
public class RoomGenerationDebugPage : DebugPage
{
    public override string Header { get; } = "Room Generation";

    protected override void RunItems(DebugMenu caller)
    {
        if (Button("Regenerate"))
            Object.FindObjectOfType<RoomGenerator>().GenerateNext();
    }
}
