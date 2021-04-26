using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the exit point of the room
/// </summary>
public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //The player has reached the room's exit
        if (collision.gameObject.IsPlayer())
        {
            //Fade out and generate a new room

            Commons.TransitionController.CreateTransitionToNextRoom();
        } 
    }
}
