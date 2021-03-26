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

        if (Toggle("Noclip", Cheats.Noclip))
            Cheats.Noclip = !Cheats.Noclip;

        if (Toggle("No Target", Cheats.NoTarget))
            Cheats.NoTarget = !Cheats.NoTarget;

        if (Toggle("Infinite Ammo", Cheats.InfiniteAmmo))
            Cheats.InfiniteAmmo = !Cheats.InfiniteAmmo;

        switch (Cheats.HealthCheat)
        {
            case Cheats.HealthCheatType.None:
                if (Button("Cheat Health: [ ]"))
                    Cheats.HealthCheat = Cheats.HealthCheatType.Godmode;
                break;

            case Cheats.HealthCheatType.Godmode:
                if (Button("Cheat Health: [God]"))
                    Cheats.HealthCheat = Cheats.HealthCheatType.BuddhaMode;
                break;

            case Cheats.HealthCheatType.BuddhaMode:
                if (Button("Cheat Health: [Buddha]"))
                    Cheats.HealthCheat = Cheats.HealthCheatType.None;
                break;
        }
        

        if (Button("Room Generation ..."))
            caller.NavigationStack.Push(new RoomGenerationDebugPage());

        if (Button("Inventory ..."))
            caller.NavigationStack.Push(new InventoryDisplayPage());
    }
}
