using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Let's the user pick up a weapon they have dropped
/// </summary>
public class DroppedWeaponPickup : WeaponPickupBase
{
    [Tooltip("The dropped weapon")]
    public WeaponInstance Weapon;

    public override WeaponInstance GetWeapon()
    {
        return Weapon;
    }
}