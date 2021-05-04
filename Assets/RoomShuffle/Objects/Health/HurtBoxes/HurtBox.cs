using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Represents a trigger with the ability to hurt hitboxes
/// </summary>
public abstract class Hurtbox : MonoBehaviour
{
    /// <summary>
    /// Whether this hurtbox will ignore a hitbox's invisibility frames
    /// </summary>
    public abstract bool IgnoresInvincibilityFrames { get; }

    /// <summary>
    /// Gets whether this hurtbox will deal damage every frame it's in contact with a hitbox. If false, the damage will only happen for one frame
    /// </summary>
    public abstract bool ContinuousDamage { get; }

    /// <summary>
    /// The type of the hurtbox. Who it will hurt
    /// </summary>
    public abstract HurtBoxTypes GetTargets();

    /// <summary>
    /// Gets how much damage the hurtbox carries
    /// </summary>
    public abstract int GetDamage(Hitbox target);

    /// <summary>
    /// Called before the hurtbox gives damage to a hitbox
    /// </summary>
    /// <param name="hitbox"></param>
    public virtual void OnDealDamage(Hitbox hitbox)
    {  }
}
