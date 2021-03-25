using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A pickup that grants the player a general key
/// </summary>
public class GeneralKeyPickup : Pickup
{
    protected override void OnPickup()
    {
        Commons.Inventory.GeneralKeys++;
    }
}