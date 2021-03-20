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
        switch (Cheats.HealthCheat)
        {
            case Cheats.HealthCheatType.None:
                Health = Mathf.Max(0, Health - rawDamage);
                break;
            case Cheats.HealthCheatType.Godmode:
                return;
            case Cheats.HealthCheatType.BuddhaMode:
                Health = Mathf.Max(1, Health - rawDamage);
                return;
        }
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
    /// Deals enough damage to kill the owner of the health 
    /// </summary>
    /// <returns></returns>
    public void Kill(bool bypassHealthCheats = false)
    {
        if (!bypassHealthCheats && Cheats.HealthCheat != Cheats.HealthCheatType.None)
            return;

        Health = 0;
    }
}
