using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public abstract class WeaponPickupBase : PickupScript
{
    public PickupBase DroppableWeaponPrefab;

    /// <summary>
    /// Gets the weapon to pick up
    /// </summary>
    /// <returns></returns>
    public abstract WeaponInstance GetWeapon();

    protected virtual void Start()
    {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = GetWeapon().Template.Icon;
        renderer.color = Color.white;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override void OnPickup()
    {
        var instance = GetWeapon();

        //Attempt to place weapon in a free slot
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (Commons.Inventory.WeaponSlots[i] == null)
            {
                Commons.Inventory.WeaponSlots[i] = instance;
                return;
            }
        }

        //If no free slots are found, drop current weapon, and replace it with the new one
        var droppedWeapon = Commons.InstantiateInCurrentLevel(DroppableWeaponPrefab, GetComponentInParent<PickupBase>().transform.position).GetComponentInChildren<DroppedWeaponPickup>();
        droppedWeapon.Weapon = Commons.Inventory.WeaponSlots[Commons.Inventory.SelectedWeaponSlot];

        
        Commons.Inventory.WeaponSlots[Commons.Inventory.SelectedWeaponSlot] = instance;
    }
}
