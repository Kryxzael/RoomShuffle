using System;
using System.Collections;
using System.Collections.Generic;
using RoomShuffle.Defaults;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool Pressed { get; set; }

    //Sprite and collision for the button down state
    private GameObject buttonDown;
    
    //Sprite and collision for the button up state
    private GameObject buttonUp;

    private MultiSoundPlayer _multiSoundPlayer;

    void Start()
    {
        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();

        //Find button up and button down children
        foreach (Transform child in transform)
        {
            if (child.name.Equals("ButtonUp"))
            {
                buttonUp = child.gameObject;
            }
            else if (child.name.Equals("ButtonDown"))
            {
                buttonDown = child.gameObject;
            }
        }

        //start the button as unpressed
        Pressed = false;
        buttonUp.SetActive(true);
        buttonDown.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Press();
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
    /// sets the button to be pressed if it isn't already
    /// </summary>
    public void Press()
    {
        if (Pressed)
            return;
        
        Pressed = true;
        buttonUp.SetActive(false);
        buttonDown.SetActive(true);
        
        //Make pressed sound
        _multiSoundPlayer.PlaySound(0,1.3f);
    }

    /// <summary>
    /// Sets the button to unpressed if it isn't already
    /// </summary>
    public void DePress()
    {
        if (!Pressed)
            return;

        Pressed = false;
        buttonUp.SetActive(true);
        buttonDown.SetActive(false);
        
        //Make depressed sound
        _multiSoundPlayer.PlaySound();
    }
    
}
