﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents an effect that can be applied to a room
/// </summary>
[Flags]
public enum RoomEffects
{
    /// <summary>
    /// No room effect will be applied
    /// </summary>
    None = 0x0,

    /// <summary>
    /// The room will have lower-than-normal gravity
    /// </summary>
    LowGravity = 0x1,

    /// <summary>
    /// The room will not have ambient lights
    /// </summary>
    Darkness = 0x2,

    /// <summary>
    /// The currency pickups in the room will be worth more than usual
    /// </summary>
    ValuePickups = 0x4,

    /// <summary>
    /// The enemies will move faster than normal
    /// </summary>
    FastFoe = 0x8,

    /// <summary>
    /// The enemies will be larger than normal
    /// </summary>
    LargeEnemies = 0x10,

    /// <summary>
    /// The projectiles will be larger than normal
    /// </summary>
    LargeProjectiles = 0x20,

    /// <summary>
    /// The player's input controls will be reversed
    /// </summary>
    ReverseControls = 0x80,

    /// <summary>
    /// The player's physics will be slippery
    /// </summary>
    Icy = 0x100,

    /// <summary>
    /// The room will have a time limit that the player must complete the room within
    /// </summary>
    Timer = 0x200,

    /// <summary>
    /// The room will spawn enemies that follow the player
    /// </summary>
    Mimic = 0x400,
}