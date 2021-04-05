using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InteractionText : MonoBehaviour
{
    private TextMeshPro TMP;
    private bool _controllerConnected = false;
    private PickupBase _pickupBase;
    
    //TODO chenge all of these to the correct sprites
    private const string KEYBOARD_SPRITE_DOWN = "<sprite=1>";
    private const string KEYBOARD_SPRITE_UP = "<sprite=0>";
    private const string CONTROLLER_SPRITE_DOWN = "<sprite=3>";
    private const string CONTROLLER_SPRITE_UP = "<sprite=2>";
    void Start()
    {
        //Time.timeScale = 0.1f;

        _pickupBase = GetComponentInParent<PickupBase>();
        
        //The interaction text shall not be visible if the player don't have to
        //pick the item up. (if ActivationMode is OnInteraction)
        if (_pickupBase.ActivationMode == PickupBase.PickupActivationMode.OnContact)
        {
            //Disable Interaction text
            gameObject.SetActive(false);
            return;
        }

        TMP = GetComponent<TextMeshPro>();

        //if a controller is connected
        if (Input.GetJoystickNames().Any())
        {
            _controllerConnected = true;
        }

        StartCoroutine(CoBlinkSprite());
    }
    
    public void Update()
    {
        //the item is within pickup range
        if (_pickupBase.InPickupRange)
        {
            //Show Interaction Text
            transform.localScale = Vector3.one;
        }
        //the item is not whithin pickup range
        else
        {
            //Hide interaction Text
            transform.localScale = Vector3.zero;
        }
    }

    /// <summary>
    /// Switch icon sprite in Interaction text 2 timer per second
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoBlinkSprite()
    {
        const float WAIT_TIME_BETWEEN_EACH_BLINK = 0.5f;

        //Show controller sprites (Y -button)
        
        string spriteUp, spriteDown;
        
        //Set sprite-text based on if a controller is plugged in
        if (_controllerConnected)
        {
            spriteUp = CONTROLLER_SPRITE_UP;
            spriteDown = CONTROLLER_SPRITE_DOWN;
        }
        else
        {
            spriteUp = KEYBOARD_SPRITE_UP;
            spriteDown = KEYBOARD_SPRITE_DOWN;
        }

        //Loop trough the two spites
        while (true)
        {
            TMP.text = String.Format("Press {0} to pick up" , spriteUp);
            yield return new WaitForSeconds(WAIT_TIME_BETWEEN_EACH_BLINK);
            TMP.text = String.Format("Press {0} to pick up" , spriteDown);
            yield return new WaitForSeconds(WAIT_TIME_BETWEEN_EACH_BLINK);   
        }
        
    }
}
