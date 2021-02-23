using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The default debug page
/// </summary>
public class HomeDebugPage : DebugPage
{
    public override string Header { get; } = "Home";

    protected override void RunItems(DebugMenu caller)
    {
        if (Toggle("Moon Jump", Cheats.MoonJump))
            Cheats.MoonJump = !Cheats.MoonJump;

        if (Button("Room Generation"))
            caller.NavigationStack.Push(new RoomGenerationDebugPage());

        if (Button("Inventory"))
            caller.NavigationStack.Push(new InventoryDisplayPage());
    }
}
