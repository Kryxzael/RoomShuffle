using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// Base class for anything that can handle a pickup
/// </summary>
public abstract class PickupScript : MonoBehaviour
{
    /// <summary>
    /// The root object of the pickup
    /// </summary>
    protected PickupBase PickupBase;

    private void Awake()
    {
        PickupBase = GetComponentInParent<PickupBase>();
    }

    /* *** */

    /// <summary>
    /// Initiates the pick-up of the item
    /// </summary>
    public void PickUp()
    {
        PickupBase.PickUp();
    }

    /// <summary>
    /// Fired when the object is picked up, before its destroyed
    /// </summary>
    public abstract void OnPickup();
}
