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

    [Header("Attack-Up")]
    [Tooltip("The amount of seconds the attack-up power-up will be active")]
    public float AttackUpTime = 15f;

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
        if (Commons.PowerUpManager.HasPowerUp(PowerUp.AttackUp))
            return (int)(baseDamage * Commons.PowerUpManager.AttackUpMultiplier);

        return baseDamage; //TODO
    }

    public override void LevelUpHealth()
    {
        base.LevelUpHealth();
        Commons.PlayerHealth.MaximumHealth = GetMaximumHealth();
        Commons.PlayerHealth.FullyHeal();
    }
}
