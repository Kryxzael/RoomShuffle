using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool Pressed { get; set; }

    private GameObject buttonDown;
    private GameObject buttonUp;
    
    void Start()
    {
        Pressed = false;

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
        
        buttonUp.SetActive(true);
        buttonDown.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Pressed = true;
        buttonUp.SetActive(false);
        buttonDown.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Pressed = false;
        buttonUp.SetActive(true);
        buttonDown.SetActive(false);
    }
}
