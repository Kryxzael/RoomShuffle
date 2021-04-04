using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base manager for the root object of pickups
/// </summary>
public sealed class PickupBase : MonoBehaviour
{
    //Whether the player is currently in range of the pickup
    private bool _inPickupRange;

    /* *** */

    [Header("Initialization")]
    [Tooltip("The price of the pickup")]
    public int Price;

    [Tooltip("How the player is able to pick up this pickup")]
    public PickupActivationMode ActivationMode;

    void Start()
    {
        if (Price > 0 && ActivationMode == PickupActivationMode.OnContact)
            Debug.LogWarning("Purchasable pickups will not work properly if ActivationMode is set to OnContact");
    }

    private void Update()
    {
        //The player is in range
        if (_inPickupRange)
        {
            //The player interacts with the item
            if (ActivationMode == PickupActivationMode.OnInteraction && Input.GetButtonDown("Interact"))
            {
                //The player has enough money to buy the item
                if (Commons.Inventory.Currency >= Price)
                {
                    PickUp();
                    Commons.Inventory.Currency -= Price;
                }
            }
                
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //The player is in contact with the item
        if (collision.gameObject.IsPlayer())
        {
            //The item is auto-picked up
            if (ActivationMode == PickupActivationMode.OnContact)
            {
                PickUp();
            }

            //The item must be manually picked up. Prime it
            else
            {
                _inPickupRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //The player leaves the item. Unprime it
        if (collision.gameObject.IsPlayer())
            _inPickupRange = false;
    }

    /// <summary>
    /// Picks up the object
    /// </summary>
    public void PickUp()
    {
        foreach (PickupScript i in GetComponentsInChildren<PickupScript>())
            i.OnPickup();

        Destroy(gameObject);
    }

    /// <summary>
    /// Represents the way an object can be picked up
    /// </summary>
    public enum PickupActivationMode
    {
        /// <summary>
        /// The object is picked up when the player touches it
        /// </summary>
        OnContact,

        /// <summary>
        /// The object is picked up when the player interacts with it
        /// </summary>
        OnInteraction
    }
}
