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

    //The weapon to use if the player tries to fire an empty weapon
    public WeaponInstance FallbackWeaponInstance;

    [Header("Weapons")]
    [Tooltip("The weapons the player is carrying")]
    public WeaponInstance[] WeaponSlots = new WeaponInstance[MAX_WEAPON_SLOTS];

    [Tooltip("The currently selected weapon slot")]
    public int SelectedWeaponSlot;

    [Tooltip("The weapon to instantiate if the player doesn't have any weapons")]
    public WeaponTemplate FallbackWeapon;

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
        set
        {
            for (int i = 0; i < WeaponSlots.Length; i++)
            {
                if (WeaponSlots[i] == value)
                {
                    SelectedWeaponSlot = i;
                    return;
                }    
            }

            throw new ArgumentException("The provided weapon is not in the player's inventory and can therefore not be selected");
        }
    }

    /// <summary>
    /// Gets the last time the player fired a weapon
    /// </summary>
    public DateTime LastFireTime { get; private set; }

    [Tooltip("The amount of spendable currency the player has")]
    public int Currency;

    [Header("Keys")]
    [Tooltip("The amount of puzzle keys the player is holding. This value is reset each room")]
    public int PuzzleKeys;

    [Tooltip("The amount of general keys the player is holding. This value is NOT reset each room")]
    public int GeneralKeys;

    private void Start()
    {
        FallbackWeaponInstance = FallbackWeapon.CreateWeaponInstance();
    }

    private void Update()
    {
        /*
         * Weapon firing
         */
        if (Input.GetButton("Fire"))
        {
            //Player does not hold a weapon, use fallback if available
            if (SelectedWeapon == null || (SelectedWeapon.Durability == 0 && !Cheats.InfiniteAmmo))
            {
                if (FallbackWeaponInstance.CanFire(ignoreDurability: true))
                {
                    FallbackWeaponInstance.Fire(this.GetPlayer().GetComponent<WeaponShooterBase>());
                    LastFireTime = DateTime.Now;
                }
                    
            }

            //Player has weapon. Fire it if it can be fired
            else
            {
                if (SelectedWeapon.CanFire(ignoreDurability: false))
                {
                    SelectedWeapon.Fire(this.GetPlayer().GetComponent<WeaponShooterBase>());
                    LastFireTime = DateTime.Now;
                }
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
