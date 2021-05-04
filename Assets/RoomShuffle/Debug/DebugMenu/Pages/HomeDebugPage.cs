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
        //Credits
        if (Button("Credits"))
            caller.NavigationStack.Push(new CreditsDebugPage());

        Separator();

        /*
         * Cheats
         */
        if (Toggle("Moon Jump", Cheats.MoonJump))
            Cheats.MoonJump = !Cheats.MoonJump;

        if (Toggle("Noclip", Cheats.Noclip))
            Cheats.Noclip = !Cheats.Noclip;

        if (Toggle("No Target", Cheats.NoTarget))
            Cheats.NoTarget = !Cheats.NoTarget;

        if (Toggle("Massive Damage", Cheats.MassiveDamage))
            Cheats.MassiveDamage = !Cheats.MassiveDamage;

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

        switch (Cheats.SwimCheat)
        {
            case Cheats.SwimCheatType.None:
                if (Button("Cheat Swimming: [ ]"))
                    Cheats.SwimCheat = Cheats.SwimCheatType.AlwaysSwim;
                break;

            case Cheats.SwimCheatType.AlwaysSwim:
                if (Button("Cheat Swimming: [Always]"))
                    Cheats.SwimCheat = Cheats.SwimCheatType.NeverSwim;
                break;

            case Cheats.SwimCheatType.NeverSwim:
                if (Button("Cheat Swimming: [Never]"))
                    Cheats.SwimCheat = Cheats.SwimCheatType.None;
                break;
        }

        /*
         * Sub-menus
         */
        if (Button("Spawn Item ..."))
            caller.NavigationStack.Push(new SpawnItemDebugPage());

        if (Button("Spawn Enemy ..."))
            caller.NavigationStack.Push(new SpawnEnemyDebugPage());

        if (Button("Room Generation ..."))
            caller.NavigationStack.Push(new RoomGenerationDebugPage());

        if (Button("Inventory ..."))
            caller.NavigationStack.Push(new InventoryDisplayPage());
    }
}
