using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A button that can activate events when held down/pressed
/// </summary>
public class Button : MonoBehaviour
{
    //Sprite and collision for the button down state
    private GameObject _buttonDown;

    //Sprite and collision for the button up state
    private GameObject _buttonUp;

    /// <summary>
    /// Gets or sets whether the button is currently pressed
    /// </summary>
    public bool Pressed { get; set; }    

    /* *** */

    private MultiSoundPlayer _multiSoundPlayer;

    void Start()
    {
        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();

        //Find button up and button down children
        foreach (Transform child in transform)
        {
            if (child.name.Equals("ButtonUp"))
            {
                _buttonUp = child.gameObject;
            }
            else if (child.name.Equals("ButtonDown"))
            {
                _buttonDown = child.gameObject;
            }
        }

        //start the button as unpressed
        Pressed = false;
        _buttonUp.SetActive(true);
        _buttonDown.SetActive(false);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        Press();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        DePress();
    }

    /// <summary>
    /// Sets the button to be pressed if it isn't already
    /// </summary>
    public virtual void Press()
    {
        if (Pressed)
            return;
        
        Pressed = true;
        _buttonUp.SetActive(false);
        _buttonDown.SetActive(true);
        
        //Make pressed sound
        _multiSoundPlayer.PlaySound(volume: 1.3f);
    }

    /// <summary>
    /// Sets the button to unpressed if it isn't already
    /// </summary>
    public virtual void DePress()
    {
        if (!Pressed)
            return;

        Pressed = false;
        _buttonUp.SetActive(true);
        _buttonDown.SetActive(false);
        
        //Make depressed sound
        _multiSoundPlayer.PlaySound();
    }
    
}
