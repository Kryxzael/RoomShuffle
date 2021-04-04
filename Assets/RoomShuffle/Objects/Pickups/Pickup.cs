using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Base class for anything that can be picked up by the player
/// </summary>
public abstract class Pickup : MonoBehaviour
{
    [Tooltip("The price of the pickup")]
    public int Price;
    
    private bool _inPickupRange;

    /* *** */

    [Tooltip("How the player is able to pick up this pickup")]
    public PickupActivationMode ActivationMode;

    /// <summary>
    /// Initiates the pick-up of the item
    /// </summary>
    public void PickUp()
    {
        OnPickup();
        Destroy(gameObject);
    }

    protected virtual void Start()
    {
        if (Price > 0 && ActivationMode == PickupActivationMode.OnContact)
        {
            Debug.Log("Pickup has price and price and has OnContact activation mode");
        }
    }

    protected virtual void Update()
    {
        //The player interacts with the item
        if (_inPickupRange && ActivationMode == PickupActivationMode.OnInteraction && Input.GetButtonDown("Interact"))
        {
            if (Commons.Inventory.Currency >= Price)
            {
                 PickUp();
                 Commons.Inventory.Currency -= Price;
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
    /// Fired when the object is picked up, before its destroyed
    /// </summary>
    protected abstract void OnPickup();

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
