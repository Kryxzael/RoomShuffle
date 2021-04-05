using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Repairs the selected weapon when picked up
/// </summary>
public class RepairPickup : PickupScript
{
    public override void OnPickup()
    {
        if (Commons.Inventory.SelectedWeapon != null)
            Commons.Inventory.SelectedWeapon.Durability = Commons.Inventory.SelectedWeapon.MaxDurability;
    }
}
