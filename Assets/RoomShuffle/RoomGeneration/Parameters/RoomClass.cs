﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents the type and purpose of a room
/// </summary>
public enum RoomClass
{
    /// <summary>
    /// The room cannot be generated naturally
    /// </summary>
    Inaccessible,

    /// <summary>
    /// A standard platforming challenge, with a normal configuration of enemies and collectibles where the goal is simply to reach the exit of the room.
    /// </summary>
    Platforming,

    /// <summary>
    /// A room without any enemies. These rooms will be relatively flat with no real obstacles. A free item or weapon can be picked up in this room.
    /// </summary>
    Respite,

    /// <summary>
    /// A room that acts as a transition between two other rooms, similarly to respite rooms, but without any items.
    /// </summary>
    Transition,

    /// <summary>
    /// A room containing more enemies than usual, and where all enemies must be killed before progressing. Enemies will not spawn with effects in this room class.
    /// </summary>
    Eradication,

    /// <summary>
    /// A room that contains a small puzzle, like collecting a key to progress or finding the right door.
    /// </summary>
    Puzzle,

    /// <summary>
    /// An extra room containing many extra bonus collectibles. These rooms will not naturally generate and can only be accessed by finding secret exits in normal rooms, or by finding locked doors.
    /// </summary>
    Secret,

    /// <summary>
    /// A room that has multiple exits, the player must choose a path and is given some information about what lies ahead beyond the different exits.
    /// </summary>
    Crossroads,

    /// <summary>
    /// A room that contains a single, large enemy that must be defeated before continuing. The enemy will have more health and deal more damage than a normal enemy.
    /// </summary>
    Boss
}