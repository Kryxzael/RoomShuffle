using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the damage and health progression of all enemies
/// </summary>
public class EnemyProgression : ProgressionController
{
    /// <summary>
    /// Gets the amount of health that an enemy with the provided base-health should have
    /// </summary>
    /// <param name="baseHealth"></param>
    /// <returns></returns>
    public int GetMaximumHealthFor(int baseHealth)
    {
        return baseHealth + 100 * HealthLevel;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <returns></returns>
    public override int GetScaledDamage(int baseDamage)
    {
        return (int)(baseDamage + 0.2f * baseDamage * DamageLevel);
    }
}
