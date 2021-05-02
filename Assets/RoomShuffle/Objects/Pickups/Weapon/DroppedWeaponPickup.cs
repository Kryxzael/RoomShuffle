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

    public Material EmptyWeaponMaterial;
    public Material NonEmptyWeaponMaterial;

    protected override void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        base.Start();

        if (Weapon.Durability == 0)
            spriteRenderer.material = EmptyWeaponMaterial;
        else
            spriteRenderer.material = NonEmptyWeaponMaterial;
    }

    public override WeaponInstance GetWeapon()
    {
        return Weapon;
    }
}