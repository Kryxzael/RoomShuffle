using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the exit point of the room
/// </summary>
public class Exit : MonoBehaviour
{
    private const float SPEEDRUN_START_TIME = 30f;

    [Tooltip("If the exit should enable speedrun mode")]
    public bool EnableSpeedRunMode = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //The player has reached the room's exit
        if (collision.gameObject.IsPlayer())
        {
            if (EnableSpeedRunMode)
            {
                Commons.SpeedRunMode = true;
                Commons.CountdownTimer.ResetCountdown(SPEEDRUN_START_TIME);
            }

            //Increase room number
            if (!Commons.RoomGenerator.CurrentRoomConfig.Class.IsSafeRoom())
                Commons.RoomGenerator.CurrentRoomNumber++;

            //Fade out and generate a new room
            Commons.TransitionController.CreateTransitionToNextRoom();
        }
    }
}
