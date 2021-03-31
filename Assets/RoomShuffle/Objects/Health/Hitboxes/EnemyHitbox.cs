using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Lets an enemy be hurt by hurtboxes
/// </summary>
public class EnemyHitbox : Hitbox
{
    private HealthController _health;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override HurtBoxTypes SusceptibleTo { get; } = HurtBoxTypes.HurtfulToEnemies;

    protected override void Awake()
    {
        base.Awake();
        _health = GetComponentInParent<HealthController>();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="hurtbox"></param>
    protected override void OnReceiveDamage(HurtBox hurtbox)
    {
        GrantInvincibilityFrames();
        _health.DealDamage(hurtbox.GetDamage());
        CreatePopNumber(hurtbox.GetDamage());

        //TODO: Temporary. Health controller should probably handle deaths
        if (_health.Health <= 0)
            Destroy(transform.parent.gameObject);
    }

    private void CreatePopNumber(int damage)
    {
        
    }
}
