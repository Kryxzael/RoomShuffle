using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of an object's health and damage leveling
/// </summary>
public abstract class ProgressionController : MonoBehaviour
{
    [Tooltip("The amount of times the health has been leveled up. Should start at zero")]
    public int HealthLevel = 0;

    [Tooltip("The amount of times the damage has been leveled up. Should start at zero")]
    public int DamageLevel = 0;

    /// <summary>
    /// Gets the amount of damage the provided base damage will deal when applied by the owner of this progression controller
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <returns></returns>
    public abstract int GetScaledDamage(int baseDamage);

    public virtual void LevelUpHealth()
    {
        HealthLevel++;

        if (this is EnemyProgression)
            Debug.Log("Enemy HP++");
    }
    
    public virtual void LevelUpDamage()
    {
        DamageLevel++;

        if (this is EnemyProgression)
            Debug.Log("Enemy DMG++");
    }
}
