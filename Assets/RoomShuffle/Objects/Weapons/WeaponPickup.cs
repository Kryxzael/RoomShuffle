using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A weapon object that can be picked up by the player
/// </summary>
public class WeaponPickup : MonoBehaviour
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            inventory.WeaponSlots[inventory.SelectedWeaponSlot] = Instance;

            Destroy(gameObject);
        }
    }
}
