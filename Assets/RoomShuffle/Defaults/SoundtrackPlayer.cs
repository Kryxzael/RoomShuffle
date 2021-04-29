using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays the game's soundtrack
/// </summary>
public class SoundtrackPlayer : MonoBehaviour
{
    public AudioSource Level1Player;
    public AudioSource Level2Player;

    private void Start()
    {
        Level1Player.Play();
        Level2Player.Play();
    }

    private void Update()
    {
        if (Commons.RoomGenerator.CurrentRoomConfig?.Class.IsSafeRoom() != false)
        {
            Level2Player.volume = 0f;
        }
        else
        {
            Level2Player.volume = 1f;
        }
    }
}
