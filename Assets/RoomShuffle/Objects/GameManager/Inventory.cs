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
    public const int MAX_WEAPON_SLOTS = 2;

    [Header("Weapons")]
    [Tooltip("The weapons the player is carrying")]
    public Weapon[] WeaponSlots = new Weapon[MAX_WEAPON_SLOTS];

    [Tooltip("The currently selected weapon slot")]
    public int SelectedWeaponSlot;

    /// <summary>
    /// Gets the weapon in the selected weapon slot
    /// </summary>
    public Weapon SelectedWeapon
    {
        get
        {
            if (SelectedWeaponSlot < 0 || SelectedWeaponSlot >= MAX_WEAPON_SLOTS)
                return null;

            return WeaponSlots[SelectedWeaponSlot];
        }
    }

    private void Start()
    {
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
        
    }

    private void OnValidate()
    {
        if (WeaponSlots.Length != MAX_WEAPON_SLOTS)
        {
            Weapon[] truncated = new Weapon[MAX_WEAPON_SLOTS];
            Array.Copy(WeaponSlots, truncated, MAX_WEAPON_SLOTS);

            WeaponSlots = truncated;
        }
    }
}
