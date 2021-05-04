using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Debug page that lets the player give themselves a weapon
/// </summary>
public class GiveWeaponDebugPage : DebugPage
{
    public override string Header { get; } = "Give Weapon";

    protected override void RunItems(DebugMenu caller)
    {
        //Cycle each weapon of the current parameter builder
        foreach (WeaponTemplate i in Commons.RoomGenerator.RoomParameterBuilder.WeaponTemplates)
        {
            if (Button(i.name))
            {
                //If a weapon is selected, give that weapon to the player
                Commons.Inventory.WeaponSlots[Commons.Inventory.SelectedWeaponSlot] = i.CreateWeaponInstance();
            }
        }
    }
}