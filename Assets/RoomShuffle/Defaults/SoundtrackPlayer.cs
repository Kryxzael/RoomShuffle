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
    //How often the channels should resyncronize with each other
    const float RESYNC_FREQUENCY = 5f;

    //How many seconds have passed since the last time the music channels have been resyncronized
    float _timeSinceMusicResync = 0f;

    //Keeps track of what objects are triggereing adrenaline, and for how long
    private readonly Dictionary<object, float> _adrenalineTriggers = new Dictionary<object, float>();

    //The player's rigidbody object (for detecting being underwater)
    private RenewableLazy<Rigidbody2D> _playerRigidbody = new RenewableLazy<Rigidbody2D>(() => CommonExtensions.GetPlayer().GetComponent<Rigidbody2D>());

    /* *** */

    /*
     * The audio-sources that play the soundtrack
     */
    [Header("Standard Soundtrack")]
    public AudioSource Level1Player;
    public AudioSource Level2Player;

    [Header("Adrenaline Soundtrack")]
    public AudioSource Level1AdrenalinePlayer;
    public AudioSource Level2AdrenalinePlayer;

    [Header("Underwater Soundtrack")]
    public AudioSource Level1UnderwaterPlayer;
    public AudioSource Level2UnderwaterPlayer;

    public MusicChannels OverrideChannels;

    private IEnumerator Start()
    {
        Level1Player.clip.LoadAudioData();
        yield return new WaitWhile(() => Level1Player.clip.loadState == AudioDataLoadState.Unloaded || Level1Player.clip.loadState == AudioDataLoadState.Loading);

        Level2Player.clip.LoadAudioData();
        yield return new WaitWhile(() => Level2Player.clip.loadState == AudioDataLoadState.Unloaded || Level2Player.clip.loadState == AudioDataLoadState.Loading);

        Level1AdrenalinePlayer.clip.LoadAudioData();
        yield return new WaitWhile(() => Level1AdrenalinePlayer.clip.loadState == AudioDataLoadState.Unloaded || Level1AdrenalinePlayer.clip.loadState == AudioDataLoadState.Loading);

        Level2AdrenalinePlayer.clip.LoadAudioData();
        yield return new WaitWhile(() => Level2AdrenalinePlayer.clip.loadState == AudioDataLoadState.Unloaded || Level2AdrenalinePlayer.clip.loadState == AudioDataLoadState.Loading);

        Level1UnderwaterPlayer.clip.LoadAudioData();
        yield return new WaitWhile(() => Level1UnderwaterPlayer.clip.loadState == AudioDataLoadState.Unloaded || Level1UnderwaterPlayer.clip.loadState == AudioDataLoadState.Loading);

        Level2UnderwaterPlayer.clip.LoadAudioData();
        yield return new WaitWhile(() => Level2UnderwaterPlayer.clip.loadState == AudioDataLoadState.Unloaded || Level2UnderwaterPlayer.clip.loadState == AudioDataLoadState.Loading);

        //Play all channels at startup
        DoForAllChannels(i => i.Play());
    }

    private void FixedUpdate()
    {
        _timeSinceMusicResync += Time.fixedDeltaTime;

        if (_timeSinceMusicResync > RESYNC_FREQUENCY)
        {
            ResyncChannels();
            _timeSinceMusicResync = 0f;
        }

        AdjustPitches();
        TickAdrenalineCounters();
        SetChannelsVolumes();
    }

    /// <summary>
    /// Attempts to fix audio desynchronization should they occur
    /// </summary>
    private void ResyncChannels()
    {
        AttemptResync(Level2Player);

        AttemptResync(Level1AdrenalinePlayer);
        AttemptResync(Level2AdrenalinePlayer);

        AttemptResync(Level1UnderwaterPlayer);
        AttemptResync(Level2UnderwaterPlayer);    
    }

    /// <summary>
    /// Attempts to syncronize the sample time of an audio source to the level 1 player
    /// </summary>
    /// <param name="target"></param>
    private void AttemptResync(AudioSource target)
    {
        target.timeSamples = Level1Player.timeSamples;
    }

    /// <summary>
    /// Decreases the adrenaline counters that are currently active and removes stale ones.
    /// </summary>
    private void TickAdrenalineCounters()
    {
        foreach (var i in _adrenalineTriggers.ToArray())
        {
            _adrenalineTriggers[i.Key] = i.Value - Time.deltaTime;

            if (_adrenalineTriggers[i.Key] <= 0)
            {
                _adrenalineTriggers.Remove(i.Key);
            }
        }

    }

    /// <summary>
    /// Calculates and sets the volume levels of the channels
    /// </summary>
    private void SetChannelsVolumes()
    {
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
        else if (_adrenalineTriggers.Any())
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

    /// <summary>
    /// Adjusts the pitches of the sound players for timers and power-ups
    /// </summary>
    private void AdjustPitches()
    {
        //By how much the pitch should increase when the timer is active
        const float PITCH_INCREMENT = 0.12f;

        //By how much the pitch changes every room in speedrun mode
        const float PITCH_INCREMENT_SMALL = 0.03f;

        /*
         * Calculate pitches
         */
        float pitch = 1f;

        //Increase speed for each room (up to 10) when speedrun mode is active
        if (Commons.SpeedRunMode)
        {
            pitch += PITCH_INCREMENT;

            for (int i = 1; i < Mathf.Min(Commons.RoomGenerator.CurrentRoomNumber, 10); i++)
                pitch += PITCH_INCREMENT_SMALL;
        }

        //Increase speed if the timer is running low
        if (Commons.CountdownTimer.TimerIsRunning && Commons.CountdownTimer.CurrentSeconds <= 10)
        {
            pitch += PITCH_INCREMENT;
        }

        //Increase speed if the timer is running lower
        if (Commons.CountdownTimer.TimerIsRunning && Commons.CountdownTimer.CurrentSeconds <= 5)
        {
            pitch += PITCH_INCREMENT;
        }

        //Decrease speed if the slow-down effect is enabled
        if (Commons.PowerUpManager.HasPowerUp(PowerUp.SlowDown))
        {
            //By how much the speed will be multiplied when the slow-down power-up is enabled
            const float SLOWDOWN_PLAYBACK_SPEED = 0.75f;
            pitch *= SLOWDOWN_PLAYBACK_SPEED;
        }

        /*
         * Apply pitch change
         */
        DoForAllChannels(i => i.pitch = pitch);
    }

    /// <summary>
    /// Runs the provided delegate on every audio source
    /// </summary>
    /// <param name="action"></param>
    private void DoForAllChannels(Action<AudioSource> action)
    {
        action(Level1Player);
        action(Level2Player);

        action(Level1AdrenalinePlayer);
        action(Level2AdrenalinePlayer);

        action(Level1UnderwaterPlayer);
        action(Level2UnderwaterPlayer);
    }

    /// <summary>
    /// Performs pre-configured linear interpolations for volumes (and sometimes pitches)
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <param name="speedMultiplier"></param>
    /// <returns></returns>
    private float LerpVolume(float current, float target, float speedMultiplier = 1f)
    {
        //Fade volume in
        if (target > current)
            return Mathf.Lerp(current, target, 0.075f * speedMultiplier);

        //Fade out
        else if (target < current)
            return Mathf.Lerp(current, target, 0.05f * speedMultiplier);

        //Make no change
        return current;
    }

    /// <summary>
    /// Registers the provided object as triggering adrenaline soundtrack for the provided amount of time
    /// </summary>
    /// <param name="key"></param>
    /// <param name="time"></param>
    public void AddAdrenalineTrigger(object key, float time = float.PositiveInfinity)
    {
        _adrenalineTriggers[key] = time;
    }

    /// <summary>
    /// Stops the provided object from triggering adrenaline soundtrack
    /// </summary>
    /// <param name="key"></param>
    /// <param name="time"></param>
    public void RemoveTrigger(object key, float afterTime = 0f)
    {
        StartCoroutine(CoRemoveTrigger());

        IEnumerator CoRemoveTrigger()
        {
            yield return new WaitForSeconds(afterTime);
            _adrenalineTriggers.Remove(key);
        }
    }


    /// <summary>
    /// Enumeration representation of the channels of the soundtrack. Supports multiple values
    /// </summary>
    [Flags]
    public enum MusicChannels
    {
        /// <summary>
        /// No channels
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The standard background channel
        /// </summary>
        Level1 = 0x1,

        /// <summary>
        /// The adrenaline foreground channel
        /// </summary>
        Level2 = 0x2,

        /// <summary>
        /// The adrenaline background channel
        /// </summary>
        Level1Adrenaline = 0x4,

        /// <summary>
        /// The standard foreground channel
        /// </summary>
        Level2Adrenaline = 0x8,

        /// <summary>
        /// The underwater background channel
        /// </summary>
        Level1Underwater = 0x10,

        /// <summary>
        /// The underwater foreground channel
        /// </summary>
        Level2Underwater = 0x20,
    }
}
