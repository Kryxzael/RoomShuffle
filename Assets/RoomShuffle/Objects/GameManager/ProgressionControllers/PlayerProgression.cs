using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the damage and health progression of the player
/// </summary>
public class PlayerProgression : ProgressionController
{
    [Tooltip("The amount of HP the player starts with")]
    public int StartingHealth = 300;

    [Tooltip("By how much, attack will scale with level")]
    public float DamageLevelScalar = 0.2f;

    /// <summary>
    /// Gets the maximum health the player should have in accordance with its health level
    /// </summary>
    public int GetMaximumHealth()
    {
        return StartingHealth + HealthController.HP_PER_HEART * HealthLevel;
    }

    private void Start()
    {
        Commons.PlayerHealth.Health = Commons.PlayerHealth.MaximumHealth = GetMaximumHealth();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <returns></returns>
    public override int GetScaledDamage(int baseDamage)
    {
        var scaledDamage = baseDamage + (DamageLevelScalar * DamageLevel * baseDamage);

        if (Commons.PowerUpManager.HasPowerUp(PowerUp.AttackUp))
            return (int)(scaledDamage * Commons.PowerUpManager.AttackUpMultiplier);

        return (int)scaledDamage;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override void LevelUpHealth()
    {
        base.LevelUpHealth();
        Commons.PlayerHealth.MaximumHealth = GetMaximumHealth();
        Commons.PlayerHealth.FullyHeal();
    }
}
