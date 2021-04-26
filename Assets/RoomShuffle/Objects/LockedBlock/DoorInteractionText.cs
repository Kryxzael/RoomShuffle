using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DoorInteractionText : MonoBehaviour
{
    private TextMeshPro TMP;
    private bool _controllerConnected = false;
    private LockedDoor _lockedDoor;
    
    //TODO change all of these to the correct sprites
    private const string KEYBOARD_SPRITE_DOWN = "<sprite=1>";
    private const string KEYBOARD_SPRITE_UP = "<sprite=0>";
    private const string CONTROLLER_SPRITE_DOWN = "<sprite=3>";
    private const string CONTROLLER_SPRITE_UP = "<sprite=2>";

    void Start()
    {
        _lockedDoor = GetComponentInParent<LockedDoor>();
        
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
        //Shows the text if the player is within
        TMP.enabled = _lockedDoor._playerInRange && Commons.Inventory.GeneralKeys > 0;
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
            TMP.text = String.Format("Press {0} to enter" , spriteUp);
            yield return new WaitForSeconds(WAIT_TIME_BETWEEN_EACH_BLINK);
            TMP.text = String.Format("Press {0} to enter" , spriteDown);
            yield return new WaitForSeconds(WAIT_TIME_BETWEEN_EACH_BLINK);   
        }
        
    }
}
