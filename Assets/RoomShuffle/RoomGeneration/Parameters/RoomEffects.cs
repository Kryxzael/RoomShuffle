using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents an effect that can be applied to a room
/// </summary>
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
    Darkness,
}