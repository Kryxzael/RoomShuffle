using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Script for handling the base of an enemy
/// </summary>
public sealed class EnemyBase : MonoBehaviour
{
    [Header("Static References")]
    public GameObject Body;
    public HealthController HealthController;
    public EnemyHitbox Hitbox;
    public EnemyContactHurtbox Hurtbox;
    public SpriteRenderer SpriteRenderer;
    public Collider2D Collider;
    public Collider2D HitboxHurtboxCollider;
    public Rigidbody2D Rigidbody;
    public Light Light;
    public Flippable Flippable;
    public GameObject HUD;

    [Header("Initialization")]
    [Tooltip("The amount of base maximum HP the enemy has")]
    public int BaseHealth = 300;

    [Tooltip("The amount of base damage the enemy's contact hitbox will deal")]
    public int BaseContactDamage = 100;

    [Tooltip("The direction the enemy will start facing.")]
    public Direction1D StartingDirection = Direction1D.Left;

    [Tooltip("The settings for how the rigid-body of the enemy will start with")]
    public RigidbodyMode RigidbodyStarupMode;

    private void Start()
    {
        HealthController.MaximumHealth = Commons.EnemyProgression.GetMaximumHealthFor(BaseHealth);
        HealthController.Health = HealthController.MaximumHealth;

        Hurtbox.ContactBaseDamage = BaseContactDamage;

        if (StartingDirection != Direction1D.None)
            Flippable.Direction = StartingDirection;

        switch (RigidbodyStarupMode)
        {
            case RigidbodyMode.Normal:
                Rigidbody.gravityScale = 1f;
                Rigidbody.bodyType = RigidbodyType2D.Dynamic;
                break;
            case RigidbodyMode.NoGravity:
                Rigidbody.gravityScale = 0f;
                Rigidbody.bodyType = RigidbodyType2D.Dynamic;
                Rigidbody.velocity = default;
                break;
            case RigidbodyMode.Static:
                Rigidbody.bodyType = RigidbodyType2D.Static;
                break;
        }
    }

    public enum RigidbodyMode
    {
        Normal,
        NoGravity,
        Static,
        DontOverride,
    }
}
