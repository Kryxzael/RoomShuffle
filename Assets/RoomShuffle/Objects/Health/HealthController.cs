using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages an object's health
/// </summary>
public class HealthController : MonoBehaviour
{
    /// <summary>
    /// The amount of health points a full heart represents
    /// </summary>
    public const int HP_PER_HEART = 100;

    [Tooltip("The maximum health of the health controller")]
    public int MaximumHealth;

    [Tooltip("The current health of the health controller")]
    public int Health;

    [Tooltip("If enabled, health cheats will affect the way this health controller consumes damage")]
    public bool AffectedByHealthCheats = false;

    [Tooltip("How much (percentage) of incoming damage will be ignored")]
    [Range(0f, 1f)]
    public float DefensePercentage;

    /// <summary>
    /// Gets the controller's health as an amount of hearts
    /// </summary>
    public float HealthAsHearts
    {
        get
        {
            return Health / HP_PER_HEART;
        }
    }

    /// <summary>
    /// Gets whether the health controller has health
    /// </summary>
    public bool IsAlive
    {
        get
        {
            return Health > 0;
        }
    }

    /// <summary>
    /// Gets whether the health controller has no health
    /// </summary>
    public bool IsDead
    {
        get
        {
            return !IsAlive;
        }
    }

    /*
     * Healing
     */

    /// <summary>
    /// Restores a certain amount of health
    /// </summary>
    /// <param name="healingPoints"></param>
    public void Heal(int healingPoints)
    {
        Health = Mathf.Min(MaximumHealth, Health + healingPoints);
    }

    /// <summary>
    /// Restores all health on the controller
    /// </summary>
    public void FullyHeal()
    {
        Health = MaximumHealth;
    }

    /*
     * Damaging
     */

    /// <summary>
    /// Deals a certain amount of raw damage
    /// </summary>
    /// <param name="rawDamage"></param>
    public void DealDamage(int rawDamage)
    {
        rawDamage = (int)(rawDamage * (1f - DefensePercentage));

        if (AffectedByHealthCheats)
        {
            switch (Cheats.HealthCheat)
            {
                case Cheats.HealthCheatType.None:
                    break;
                case Cheats.HealthCheatType.Godmode:
                    return;
                case Cheats.HealthCheatType.BuddhaMode:
                    Health = Mathf.Max(1, Health - rawDamage);
                    return;
            }
        }

        Health = Mathf.Max(0, Health - rawDamage);
    }

    /// <summary>
    /// Deals a certain amount of base damage scaled by the provided progression manager
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <param name="fromWhom"></param>
    public void DealScaledDamage(int baseDamage, ProgressionController fromWhom)
    {
        DealDamage(fromWhom.GetScaledDamage(baseDamage));
    }

    /// <summary>
    /// Deal a third of the health controller's maximum health as damage
    /// </summary>
    public void SoftKill()
    {
        DealDamage(GetSoftDeathDamage());
    }

    /// <summary>
    /// Deals enough damage to kill the owner of the health 
    /// </summary>
    /// <returns></returns>
    public void Kill(bool bypassHealthCheats = false)
    {
        if (!bypassHealthCheats && Cheats.HealthCheat != Cheats.HealthCheatType.None)
            return;

        Health = 0;
    }

    /// <summary>
    /// Gets the amount of damage that a soft kill of this manager will deal
    /// </summary>
    /// <returns></returns>
    public int GetSoftDeathDamage()
    {
        return MaximumHealth / 3;
    }
}
