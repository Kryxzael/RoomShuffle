using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A weapon object that can be picked up by the player
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class WeaponPickup : PickupScript
{
    [Tooltip("The weapon that will be created for this pickup if enabled")]
    public WeaponTemplate Template;

    [Tooltip("Whether a weapon instance should be manually created")]
    public bool ShouldCreateWeaponFromTemplate = false;

    //Template

    /* *** */

    //Instance

    /// <summary>
    /// Get or sets the weapon that will be provided to the player when picked up
    /// </summary>
    public WeaponInstance Instance { get; set; }

    private void Start()
    {
        if (ShouldCreateWeaponFromTemplate)
            Instance = Template.CreateWeaponInstance();

        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Instance.Template.Icon;
        renderer.color = Color.white;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override void OnPickup()
    {
        //Attempt to place weapon in a free slot
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (Commons.Inventory.WeaponSlots[i] == null)
            {
                Commons.Inventory.WeaponSlots[i] = Instance;
                return;
            }
        }
    
        //If no free slots are found, overwrite the existing one
        Commons.Inventory.WeaponSlots[Commons.Inventory.SelectedWeaponSlot] = Instance;
        //TODO: Drop the current weapon in this case
    }
}
