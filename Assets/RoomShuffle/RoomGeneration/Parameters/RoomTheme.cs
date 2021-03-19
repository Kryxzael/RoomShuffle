using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents a room's tileset, audio and other visuals
/// </summary>
public enum RoomTheme
{
    /// <summary>
    /// The graphics used when editing rooms
    /// </summary>
    Edit,

    /// <summary>
    /// A standard grassy plains theme
    /// </summary>
    Grass,

    /// <summary>
    /// A snowy version of the grass theme
    /// </summary>
    Snow,

    /// <summary>
    /// A version of the grass theme with fallen leaves and red grass
    /// </summary>
    Autumn,

    /// <summary>
    /// An underground area with a rocky tileset
    /// </summary>
    Cave,

    /// <summary>
    /// An area with red volcanic stone
    /// </summary>
    Volcano
}