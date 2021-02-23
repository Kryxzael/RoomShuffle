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
        Inventory inventory = UnityEngine.Object.FindObjectOfType<Inventory>();

        for (int i = 0; i < inventory.WeaponSlots.Length; i++)
        {
            string name = $"{inventory.WeaponSlots[i].name} [{inventory.WeaponSlots[i].Durability} / {inventory.WeaponSlots[i].MaxDurability}]";

            if (i == inventory.SelectedWeaponSlot)
                name = "*" + name + "*";

            if (Button(name))
                inventory.SelectedWeaponSlot = i;
        }
    }
}