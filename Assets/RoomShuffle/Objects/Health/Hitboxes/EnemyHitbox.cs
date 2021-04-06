using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;

/// <summary>
/// Lets an enemy be hurt by hurtboxes
/// </summary>
public class EnemyHitbox : Hitbox
{
    private HealthController _health;

    /* *** */
    public PopNumber DamageTextPrefab;

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
    /// Creates a red floating label over the enemy's head to signify the damage it took
    /// </summary>
    /// <param name="damage"></param>
    public void CreateDamagePopNumber(int damage)
    {
        const float POP_NUMBER_RANDOM_X_OFFSET = 1f;

        float verticalOffset = 1f;
        float horizontalOffset = RandomValueBetween.Symetrical(POP_NUMBER_RANDOM_X_OFFSET).Pick();

        TextMeshPro instance = Commons.InstantiateInCurrentLevel(
            original: DamageTextPrefab,
            position: transform.position + Vector3.up * verticalOffset + Vector3.right * horizontalOffset
        ).GetComponent<TextMeshPro>();

        instance.text = $"-{damage}";
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="hurtbox"></param>
    protected override void OnReceiveDamage(HurtBox hurtbox)
    {
        //Dead men don't scream
        if (_health.IsDead)
            return;

        //Give I-frames
        GrantInvincibilityFrames();

        //Deal damage
        int damage = hurtbox.GetDamage(this);
        _health.DealDamage(damage);

        //Create damage pop-up
        CreateDamagePopNumber(damage);
        


        //TODO: Temporary. Health controller should probably handle deaths
        if (_health.IsDead)
        {
            //Drop items
            foreach (var i in GetComponentsInChildren<DropLootTableOnDeath>())
                i.DropItem();

            //Destroy the object
            Destroy(transform.parent.gameObject);
        }
            
    }
}
