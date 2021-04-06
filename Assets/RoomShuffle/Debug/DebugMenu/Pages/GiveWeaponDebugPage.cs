using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GiveWeaponDebugPage : DebugPage
{
    public override string Header { get; } = "Give Weapon";

    protected override void RunItems(DebugMenu caller)
    {
        foreach (WeaponTemplate i in Commons.RoomGenerator.RoomParameterBuilder.WeaponTemplates)
        {
            if (Button(i.name))
            {
                Commons.Inventory.WeaponSlots[Commons.Inventory.SelectedWeaponSlot] = i.CreateWeaponInstance();
            }
        }
    }
}