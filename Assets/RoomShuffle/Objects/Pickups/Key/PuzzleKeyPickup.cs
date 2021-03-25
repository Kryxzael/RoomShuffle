using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A pickup that grants the player a puzzle key
/// </summary>
public class PuzzleKeyPickup : Pickup
{
    protected override void OnPickup()
    {
        Commons.Inventory.PuzzleKeys++;
    }
}