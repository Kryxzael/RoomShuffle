using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Plays the game's soundtrack
/// </summary>
public class SoundtrackPlayer : MonoBehaviour
{
    public AudioSource Level1Player;
    public AudioSource Level2Player;

    public AudioSource Level1AdrenalinePlayer;
    public AudioSource Level2AdrenalinePlayer;

    public AudioSource Level1UnderwaterPlayer;
    public AudioSource Level2UnderwaterPlayer;

    private readonly Dictionary<object, float> AdrenalineTriggers = new Dictionary<object, float>();

    private RenewableLazy<Rigidbody2D> _playerRigidbody = new RenewableLazy<Rigidbody2D>(() =>
    {
        var player = CommonExtensions.GetPlayer();

        if (player)
            return player.GetComponent<Rigidbody2D>();

        return null;
    });

    public MusicChannels OverrideChannels;

    private void Start()
    {
        DoForAllChannels(i => i.Play());
    }

    private void Update()  
    {
        const float PITCH_INCREMENT = 0.12f;
        const float PITCH_INCREMENT_SMALL = 0.03f;

        DoForAllChannels(i => i.pitch = 1f);

        if (Commons.SpeedRunMode)
        {
            DoForAllChannels(i => i.pitch += PITCH_INCREMENT);

            for (int i = 1; i < Mathf.Max(Commons.RoomGenerator.CurrentRoomNumber, 10); i++)
                DoForAllChannels(i => i.pitch += PITCH_INCREMENT_SMALL);
        }

        if (Commons.CountdownTimer.TimerIsRunning && Commons.CountdownTimer.CurrentSeconds <= 10)
        {
            DoForAllChannels(i => i.pitch += PITCH_INCREMENT);
        }

        if (Commons.CountdownTimer.TimerIsRunning && Commons.CountdownTimer.CurrentSeconds <= 5)
        {
            DoForAllChannels(i => i.pitch += PITCH_INCREMENT);
        }

        if (Commons.PowerUpManager.HasPowerUp(PowerUp.SlowDown))
        {
            const float SLOWDOWN_PLAYBACK_SPEED = 0.75f;
            DoForAllChannels(i => i.pitch *= SLOWDOWN_PLAYBACK_SPEED);
        }

        foreach (var i in AdrenalineTriggers.ToArray())
        {
            AdrenalineTriggers[i.Key] = i.Value - Time.deltaTime;

            if (AdrenalineTriggers[i.Key] <= 0)
            {
                AdrenalineTriggers.Remove(i.Key);
            }
        }

        if (OverrideChannels != MusicChannels.None)
        {
            Level1Player.volume = LerpVolume(Level1Player.volume, OverrideChannels.HasFlag(MusicChannels.Level1) ? 1f : 0f);
            Level2Player.volume = LerpVolume(Level2Player.volume, OverrideChannels.HasFlag(MusicChannels.Level2) ? 1f : 0f);
            Level1AdrenalinePlayer.volume = LerpVolume(Level1AdrenalinePlayer.volume, OverrideChannels.HasFlag(MusicChannels.Level1Adrenaline) ? 1f : 0f);
            Level2AdrenalinePlayer.volume = LerpVolume(Level2AdrenalinePlayer.volume, OverrideChannels.HasFlag(MusicChannels.Level2Adrenaline) ? 1f : 0f);
            Level1UnderwaterPlayer.volume = LerpVolume(Level1UnderwaterPlayer.volume, OverrideChannels.HasFlag(MusicChannels.Level1Underwater) ? 1f : 0f);
            Level2UnderwaterPlayer.volume = LerpVolume(Level2UnderwaterPlayer.volume, OverrideChannels.HasFlag(MusicChannels.Level2Underwater) ? 1f : 0f);
        }

        //Play underwater music
        else if (_playerRigidbody.Value && Water.IsSubmerged(_playerRigidbody.Value))
        {
            Level1UnderwaterPlayer.volume = LerpVolume(Level1UnderwaterPlayer.volume, 1f);
            Level1Player.volume = LerpVolume(Level1Player.volume, 0f);
            Level2Player.volume = LerpVolume(Level2Player.volume, 0f);
            Level1AdrenalinePlayer.volume = LerpVolume(Level1AdrenalinePlayer.volume, 0f);
            Level2AdrenalinePlayer.volume = LerpVolume(Level2AdrenalinePlayer.volume, 0f);

            if (Commons.RoomGenerator.CurrentRoomConfig?.Class.IsSafeRoom() != false)
            {
                Level2UnderwaterPlayer.volume = LerpVolume(Level2UnderwaterPlayer.volume, 0f);
            }
            else
            {
                Level2UnderwaterPlayer.volume = LerpVolume(Level2UnderwaterPlayer.volume, 1f);
            }
        }

        //Play adrenaline music
        else if (AdrenalineTriggers.Any())
        {
            Level1AdrenalinePlayer.volume = LerpVolume(Level1AdrenalinePlayer.volume, 1f);
            Level1Player.volume = LerpVolume(Level1Player.volume, 0f);
            Level2Player.volume = LerpVolume(Level2Player.volume, 0f);
            Level1UnderwaterPlayer.volume = LerpVolume(Level1UnderwaterPlayer.volume, 0f);
            Level2UnderwaterPlayer.volume = LerpVolume(Level2UnderwaterPlayer.volume, 0f);

            if (Commons.RoomGenerator.CurrentRoomConfig?.Class.IsSafeRoom() != false)
            {
                Level2AdrenalinePlayer.volume = LerpVolume(Level2AdrenalinePlayer.volume, 0f);
            }
            else
            {
                Level2AdrenalinePlayer.volume = LerpVolume(Level2AdrenalinePlayer.volume, 1f);
            }
        }

        //Play normal music
        else
        {
            Level1Player.volume = LerpVolume(Level1Player.volume, 1f);
            Level1AdrenalinePlayer.volume = LerpVolume(Level1AdrenalinePlayer.volume, 0f);
            Level2AdrenalinePlayer.volume = LerpVolume(Level2AdrenalinePlayer.volume, 0f);
            Level1UnderwaterPlayer.volume = LerpVolume(Level1UnderwaterPlayer.volume, 0f);
            Level2UnderwaterPlayer.volume = LerpVolume(Level2UnderwaterPlayer.volume, 0f);

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

    private void DoForAllChannels(Action<AudioSource> action)
    {
        action(Level1Player);
        action(Level2Player);

        action(Level1AdrenalinePlayer);
        action(Level2AdrenalinePlayer);

        action(Level1UnderwaterPlayer);
        action(Level2UnderwaterPlayer);
    }
    private float LerpVolume(float current, float target, float speedMultiplier = 1f)
    {
        if (target > current)
            return Mathf.Lerp(current, target, 0.025f * speedMultiplier);

        else if (target < current)
            return Mathf.Lerp(current, target, 0.01f * speedMultiplier);

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

    [Flags]
    public enum MusicChannels
    {
        None = 0x0,
        Level1 = 0x1,
        Level2 = 0x2,
        Level1Adrenaline = 0x4,
        Level2Adrenaline = 0x8,
        Level1Underwater = 0x10,
        Level2Underwater = 0x20,
    }
}
