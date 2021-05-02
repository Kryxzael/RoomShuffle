using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Holds the game's enabled debug cheat codes
/// </summary>
public static class Cheats
{
    /// <summary>
    /// If enabled, the player can always jump. Even if they're not grounded
    /// </summary>
    public static bool MoonJump { get; set; }

    /// <summary>
    /// If enabled, the player will not have any physics or collision
    /// </summary>
    public static bool Noclip { get; set; }

    /// <summary>
    /// If enabled, targeting enemies will not see the player
    /// </summary>
    public static bool NoTarget { get; set; }

    /// <summary>
    /// If enabled, ammunition will not be depleted when weapons are fired, and you can fire with zero ammo
    /// </summary>
    public static bool InfiniteAmmo { get; set; }

    /// <summary>
    /// If enabled, any projectile fired by the player will deal virtually infinite damage
    /// </summary>
    public static bool MassiveDamage { get; set; }

    /// <summary>
    /// Sets the health cheat of the player
    /// </summary>
    public static HealthCheatType HealthCheat { get; set; } = HealthCheatType.None;

    /// <summary>
    /// Sets the swim cheat
    /// </summary>
    public static SwimCheatType SwimCheat { get; set; } = SwimCheatType.None;

    /// <summary>
    /// Represents a way to cheat the player's health
    /// </summary>
    public enum HealthCheatType
    {
        /// <summary>
        /// No health cheat is enabled
        /// </summary>
        None,

        /// <summary>
        /// The player cannot take damage or receive any negative effects
        /// </summary>
        Godmode,

        /// <summary>
        /// The player's health will never drop below 1
        /// </summary>
        BuddhaMode
    }

    /// <summary>
    /// Represents a way to cheat the game's swimming mechanics
    /// </summary>
    public enum SwimCheatType
    {
        /// <summary>
        /// No swim cheat is enabled
        /// </summary>
        None,

        /// <summary>
        /// The game will always treat objects as if they were underwater
        /// </summary>
        AlwaysSwim,

        /// <summary>
        /// The game will never treat objects as if they were underwater
        /// </summary>
        NeverSwim
    }
}
