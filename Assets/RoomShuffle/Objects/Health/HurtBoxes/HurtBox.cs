using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Represents a trigger with the ability to hurt something
/// </summary>
public abstract class HurtBox : MonoBehaviour
{
    /// <summary>
    /// The type of the hurtbox. Who it will hurt
    /// </summary>
    public abstract HurtBoxTypes GetTargets();

    /// <summary>
    /// Gets how much damage the hurtbox carries
    /// </summary>
    public abstract int GetDamage();
}
