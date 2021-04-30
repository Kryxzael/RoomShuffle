using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Plays the game's soundtrack
/// </summary>
public class SoundtrackPlayer : MonoBehaviour
{
    public AudioSource Level1Player;
    public AudioSource Level2Player;

    public AudioSource Level1AdrenalinePlayer;
    public AudioSource Level2AdrenalinePlayer;

    private readonly Dictionary<object, float> AdrenalineTriggers = new Dictionary<object, float>();

    private void Start()
    {
        Level1Player.Play();
        Level2Player.Play();

        Level1AdrenalinePlayer.Play();
        Level2AdrenalinePlayer.Play();

    }

    private void Update()
    {
        foreach (var i in AdrenalineTriggers.ToArray())
        {
            AdrenalineTriggers[i.Key] = i.Value - Time.deltaTime;

            if (AdrenalineTriggers[i.Key] <= 0)
            {
                AdrenalineTriggers.Remove(i.Key);
            }
        }

        if (AdrenalineTriggers.Any())
        {
            Level1AdrenalinePlayer.volume = LerpVolume(Level1AdrenalinePlayer.volume, 1f);
            Level1Player.volume = LerpVolume(Level1Player.volume, 0f);
            Level2Player.volume = LerpVolume(Level2Player.volume, 0f);

            if (Commons.RoomGenerator.CurrentRoomConfig?.Class.IsSafeRoom() != false)
            {
                Level2AdrenalinePlayer.volume = LerpVolume(Level1AdrenalinePlayer.volume, 0f);
            }
            else
            {
                Level2AdrenalinePlayer.volume = LerpVolume(Level2AdrenalinePlayer.volume, 1f);
            }
        }
        else
        {
            Level1Player.volume = LerpVolume(Level1Player.volume, 1f);
            Level1AdrenalinePlayer.volume = LerpVolume(Level1AdrenalinePlayer.volume, 0f);
            Level2AdrenalinePlayer.volume = LerpVolume(Level2AdrenalinePlayer.volume, 0f);

            if (Commons.RoomGenerator.CurrentRoomConfig?.Class.IsSafeRoom() != false)
            {
                Level2Player.volume = LerpVolume(Level2Player.volume, 0f);
            }
            else
            {
                Level2Player.volume = LerpVolume(Level2Player.volume, 1f);
            }
        }
        
    }
    private float LerpVolume(float current, float target)
    {
        if (target > current)
            return Mathf.Lerp(current, target, 0.025f);

        else if (target < current)
            return Mathf.Lerp(current, target, 0.01f);

        return current;
    }

    public void AddTrigger(object key, float time = float.PositiveInfinity)
    {
        AdrenalineTriggers[key] = time;
    }

    public void RemoveTrigger(object key, float afterTime = 0f)
    {
        StartCoroutine(CoRemoveTrigger());

        IEnumerator CoRemoveTrigger()
        {
            yield return new WaitForSeconds(afterTime);
            AdrenalineTriggers.Remove(key);
        }
    }
}
