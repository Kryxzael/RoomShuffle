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
        ReadOnly($"<3: {Commons.PlayerHealth.Health} / {Commons.PlayerHealth.MaximumHealth}");
        ReadOnly($"${Commons.Inventory.Currency}");
        ReadOnly($"{Commons.Inventory.GeneralKeys} general keys");
        ReadOnly($"{Commons.Inventory.PuzzleKeys} puzzle keys");

        for (int i = 0; i < Commons.Inventory.WeaponSlots.Length; i++)
        {
            WeaponInstance instance = Commons.Inventory.WeaponSlots[i];

            if (instance == null)
            {
                ReadOnly("(none)");
            }
            else
            {
                string name = $"{instance.Template.name} [{instance.Durability} / {instance.MaxDurability}]";

                if (Cheats.InfiniteAmmo)
                    name = $"{instance.Template.name} [∞ / ∞]";

                if (i == Commons.Inventory.SelectedWeaponSlot)
                    name = "*" + name + "*";

                if (Button(name))
                    Commons.Inventory.SelectedWeaponSlot = i;
            }
        }
    }
}