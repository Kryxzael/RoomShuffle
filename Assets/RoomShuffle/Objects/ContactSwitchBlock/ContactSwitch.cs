using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// A switch that is activated/deactivated on collision with the player
/// </summary>
public class ContactSwitch : MonoBehaviour
{
    //Is the state of the switch locked
    private bool _locked;

    /* *** */

    private SpriteRenderer _sprite;

    /* *** */

    [Tooltip("If the block should start 'On'")]
    public bool On = false;
    
    [Header("Sprites")]
    public Sprite OnSprite;
    public Sprite OffSprite;
    public Sprite NeutralSprite;
    
    [Header("Trigger sides")]
    public bool TriggerOnTop = true;
    public bool TriggerOnSides = true;
    public bool TriggerOnBottom = true;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        
        //Set the sprite corresponding to the "On" variable
        _sprite.sprite = On ? OnSprite : OffSprite;

        /*
         * Sets configurations based on the trigger sides
         */
        string enabledChildObject;

        if (TriggerOnTop && !TriggerOnSides && !TriggerOnBottom)
        {
            enabledChildObject = "Top";
        }

        else if (!TriggerOnTop && TriggerOnSides && !TriggerOnBottom)
        {
            enabledChildObject = "Sides";
        }

        else if (TriggerOnTop && TriggerOnSides && !TriggerOnBottom)
        {
            enabledChildObject = "TopAndSides";
        }

        else if (!TriggerOnTop && TriggerOnSides && TriggerOnBottom)
        {
            enabledChildObject = "BottomAndSides";
        }
        
        else if (!TriggerOnTop && !TriggerOnSides && TriggerOnBottom)
        {
            enabledChildObject = "Bottom";
        }
        
        else if (TriggerOnTop && !TriggerOnSides && TriggerOnBottom)
        {
            enabledChildObject = "TopAndBottom";
        }
        else
        {
            enabledChildObject = "All";
        }

        //Enable child object
        transform.Cast<Transform>().Single(i => i.name == enabledChildObject).gameObject.SetActive(true);
    }

    /// <summary>
    /// Function called by children when trigger is entered
    /// </summary>
    /// <param name="collider"></param>
    public void TriggerSwitch(Collider2D collider)
    {
        if (_locked)
            return;

        //Change state if the collision object is the player
        if (collider.gameObject.IsPlayer())
        {
            On = !On;

            //change corresponding sprite
            _sprite.sprite = On ? OnSprite : OffSprite;
            
        }
    }

    /// <summary>
    /// Locks the block so that it can't be switched anymore.
    /// </summary>
    public void Lock()
    {
        _sprite.sprite = NeutralSprite;
        _locked = true;
    }
}
