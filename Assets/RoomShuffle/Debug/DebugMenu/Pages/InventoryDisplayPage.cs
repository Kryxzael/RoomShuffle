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
    private readonly RenewableLazy<Inventory> _inventory = new RenewableLazy<Inventory>(() => UnityEngine.Object.FindObjectOfType<Inventory>());

    public override string Header { get; } = "Inventory";

    protected override void RunItems(DebugMenu caller)
    {
        for (int i = 0; i < _inventory.Value.WeaponSlots.Length; i++)
        {
            WeaponInstance instance = _inventory.Value.WeaponSlots[i];

            if (instance == null)
            {
                ReadOnly("(none)");
            }
            else
            {
                string name = $"{instance.Template.name} [{instance.Durability} / {instance.MaxDurability}]";

                if (i == _inventory.Value.SelectedWeaponSlot)
                    name = "*" + name + "*";

                if (Button(name))
                    _inventory.Value.SelectedWeaponSlot = i;
            }
        }
    }
}