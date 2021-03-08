using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents one or more classes of objects a hurtbox will hurt
/// </summary>
[Flags]
public enum HurtBoxTypes
{
    /// <summary>
    /// The hurtbox will not hurt anyone. This configuration is useless and is only here for the sake of enum flags
    /// </summary>
    None = 0x0,

    /// <summary>
    /// The hurtbox will hurt the player
    /// </summary>
    HurtfulToPlayer = 0x1,

    /// <summary>
    /// The hurtbox will hurt the enemies
    /// </summary>

    HurtfulToEnemies = 0x2,
}
