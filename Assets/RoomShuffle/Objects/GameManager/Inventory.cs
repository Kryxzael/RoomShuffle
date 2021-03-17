using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Handles the state of the player's inventory
/// </summary>
public class Inventory : MonoBehaviour
{
    //The amount of weapons the player can carry
    public const int MAX_WEAPON_SLOTS = 2;

    [Header("Weapons")]
    [Tooltip("The weapons the player is carrying")]
    public WeaponInstance[] WeaponSlots = new WeaponInstance[MAX_WEAPON_SLOTS];

    [Tooltip("The currently selected weapon slot")]
    public int SelectedWeaponSlot;

    /// <summary>
    /// Gets the weapon in the selected weapon slot
    /// </summary>
    public WeaponInstance SelectedWeapon
    {
        get
        {
            if (SelectedWeaponSlot < 0 || SelectedWeaponSlot >= MAX_WEAPON_SLOTS)
                return null;

            return WeaponSlots[SelectedWeaponSlot];
        }
    }

    private void Update()
    { 
        /*
         * Weapon firing
         */
        if (SelectedWeapon != null)
        {
            if (Input.GetButton("Fire") && SelectedWeapon.CanFire())
            {
                SelectedWeapon.Fire(this.GetPlayer().GetComponent<WeaponShooter>());
            }
        }
        /*
         * Switch weapon with button
         */
        if (Input.GetButtonDown("QuickSwitchWeapon"))
        {
            SelectedWeaponSlot++;
            if (SelectedWeaponSlot >= MAX_WEAPON_SLOTS)
            {
                SelectedWeaponSlot -= MAX_WEAPON_SLOTS;
            }
        }
        if (Input.GetButtonDown("SwitchToWeapon1"))
        {
            SelectedWeaponSlot = 0;
        }
        if (Input.GetButtonDown("SwitchToWeapon2"))
        {
            SelectedWeaponSlot = 1;
        }

        
    }
}
