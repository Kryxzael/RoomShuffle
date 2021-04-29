using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Displays the player's current inventory as a debug page
/// </summary>
public class InventoryDisplayPage : DebugPage
{
    public override string Header { get; } = "Inventory";

    protected override void RunItems(DebugMenu caller)
    {
        if (Button("Levels and Health ..."))
        {
            caller.NavigationStack.Push(new LevelsDebugPage());
        }

        if (Button("Give Weapon ..."))
        {
            caller.NavigationStack.Push(new GiveWeaponDebugPage());
        }

        if (Button("Give Powerup ..."))
        {
            caller.NavigationStack.Push(new GivePowerUpDebugPage());
        }

        Separator();

        if (Button("Give $$$"))
        {
            Commons.Inventory.Currency += 50;
        }

        if (Button("Give General Key"))
        {
            Commons.Inventory.GeneralKeys++;
        }

        if (Button("Give Puzzle Key"))
        {
            Commons.Inventory.PuzzleKeys++;
        }
    }
}