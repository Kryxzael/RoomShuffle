using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Manages the player's active power-up
/// </summary>
public class PowerUpManager : MonoBehaviour
{
    private Dictionary<PowerUp, float> _powerUpsWithTimers = new Dictionary<PowerUp, float>();

    [Header("Power-Up Settings")]
    [Tooltip("By what factor to multiply the player's base damage when Attack-Up is active")]
    public float AttackUpMultiplier = 2f;

    [Tooltip("The timescale of the game in slow-down mode")]
    public float SlowDownTimeScale = 0f;

    [Tooltip("By what factor to multiply the player's movement speed when invincible")]
    public float InvincibilitySpeedMultiplier = 1.5f;

    [Tooltip("By what factor to multiply the player's jumping speed when invincible")]
    public float InvincibilityJumpMultiplier = 1.5f;

    [Tooltip("The amount (percentage) of damage a player with the defense-up power-up will absorb")]
    [Range(0f, 1f)]
    public float DefenseStrength = 0.5f;

    [Header("Max Times")]
    [Tooltip("How long the player will have the attack-up power-up for")]
    public float AttackUpTime = 15f;

    [Tooltip("How long the player will have the slowdown power-up for")]
    public float SlowDownUnscaledTime = 15f;

    [Tooltip("How long the player will have the defense-up power-up for")]
    public float DefenseUpTime = 30f;

    [Tooltip("How long the player will have the invincibility power-up for")]
    public float InvincibilityTime = 15f;

    private void Update() 
    {
        //Decrease timers (unless in a safe-room
        if (!Commons.RoomGenerator.CurrentRoomConfig.Class.IsSafeRoom())
        {
            foreach (var (key, value) in _powerUpsWithTimers.Select(i => (i.Key, i.Value)).ToArray())
                _powerUpsWithTimers[key] = Math.Max(0f, value - Time.unscaledDeltaTime);
        }

        if (HasPowerUp(PowerUp.AttackUp))
        {
            Commons.SoundtrackPlayer.AddTrigger(PowerUp.AttackUp, 0.5f);
        }

        if (HasPowerUp(PowerUp.Invincibility))
        {
            Commons.PlayerHealth.DefensePercentage = 1f;
            Commons.SoundtrackPlayer.AddTrigger(PowerUp.Invincibility, 0.5f);
        }
            

        else if (HasPowerUp(PowerUp.DefenseUp))
            Commons.PlayerHealth.DefensePercentage = DefenseStrength;

        else
            Commons.PlayerHealth.DefensePercentage = 0f;
    }

    /// <summary>
    /// Gives the player a power-up
    /// </summary>
    /// <param name="powerup"></param>
    public void GrantPowerUp(PowerUp powerup)
    {
        _powerUpsWithTimers[powerup] = GetMaximumTime(powerup);
    }

    /// <summary>
    /// Grants a power-up with infinite duration
    /// </summary>
    /// <param name="i"></param>
    public void GrantPermanentPowerup(PowerUp powerup)
    {
        _powerUpsWithTimers[powerup] = float.PositiveInfinity;
    }

    /// <summary>
    /// Removes a power-up
    /// </summary>
    /// <param name="powerup"></param>
    public void RevokePowerup(PowerUp powerup)
    {
        _powerUpsWithTimers[powerup] = 0f;
    }

    /// <summary>
    /// Gets how much time a specific power-up has left
    /// </summary>
    /// <param name="powerup"></param>
    public float GetTimeLeft(PowerUp powerup)
    {
        if (_powerUpsWithTimers.ContainsKey(powerup))
            return _powerUpsWithTimers[powerup];

        return 0f;
    }

    /// <summary>
    /// Gets the time the provided power-up has when it is activated
    /// </summary>
    /// <param name="powerup"></param>
    /// <returns></returns>
    public float GetMaximumTime(PowerUp powerup)
    {
        return powerup switch
        {
            PowerUp.AttackUp => AttackUpTime,
            PowerUp.SlowDown => SlowDownUnscaledTime,
            PowerUp.DefenseUp => AttackUpTime,
            PowerUp.Invincibility => InvincibilityTime,
            _ => 0,
        };
    }

    /// <summary>
    /// Gets whether the player has the provided power-up
    /// </summary>
    /// <param name="powerup"></param>
    /// <returns></returns>
    public bool HasPowerUp(PowerUp powerup)
    {
        return GetTimeLeft(powerup) > 0;
    }
}

/// <summary>
/// Represents the player's power-ups
/// </summary>
public enum PowerUp
{
    /// <summary>
    /// Increases the player's attack output
    /// </summary>
    AttackUp,

    /// <summary>
    /// Reduces the timescale of the game
    /// </summary>
    SlowDown,

    /// <summary>
    /// Reduces the damage the player takes
    /// </summary>
    DefenseUp,

    /// <summary>
    /// Makes the player invincible, and lets the kill enemies on touch
    /// </summary>
    Invincibility
}
