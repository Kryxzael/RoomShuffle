using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public abstract class Projectile : MonoBehaviour
{

    private WeaponInstance _weaponInstance;
    private Vector3 _projectileSpwanPoint;
    private WeaponFireHurtbox _hurtBox;

    public float Speed;

    /// <summary>
    /// Gets or sets the direction the projectile will travel in
    /// </summary>
    public Vector2 Direction
    {
        get
        {
            return transform.up;
        }
        set
        {
            transform.up = value;
            transform.eulerAngles = new Vector3(0f, 0f, transform.rotation.z);
        }
    }

    /// <summary>
    /// Gets whether the projectile should be destroyed when it hits a solid object
    /// </summary>
    public abstract bool DestroyOnGroundImpact { get; }

    /// <summary>
    /// Gets whether the projectile should be destroyed when it hits and deals damage to a hitbox
    /// </summary>
    public abstract bool DestroyOnHitboxContact { get; }

    /// <summary>
    /// Gets whether the projectile is outside its range and should be destroyed
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsOutOfRange()
    {
        return Vector2.Distance(transform.position, _projectileSpwanPoint) > _weaponInstance.Range;
    }

    protected virtual void Update()
    {
        if (IsOutOfRange())
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Start()
    {
        transform.localScale = new Vector3(
            x: Commons.GetEffectValue(transform.localScale.x, EffectValueType.ProjectileSize),
            y: Commons.GetEffectValue(transform.localScale.y, EffectValueType.ProjectileSize),
            z: Commons.GetEffectValue(transform.localScale.z, EffectValueType.ProjectileSize)
        );

        _hurtBox = GetComponentInChildren<WeaponFireHurtbox>();
        _weaponInstance = _hurtBox.Weapon;
        _projectileSpwanPoint = transform.position;
        
        //Old code for _projectileSpawnPoint beneath
        // _projectileSpwanPoint = hurtBox.Shooter.GetProjectilesSpawnPoint();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //The bullet has hit a wall
        if (DestroyOnGroundImpact)
        {
            Destroy(gameObject);
            return;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //The bullet collided with its shooter: Ignore.
        if (collision.gameObject.GetComponentInParent<WeaponShooterBase>() == _hurtBox.Shooter)
            return;

        //The bullet collided with another bullet of the same shooter
        if (collision.gameObject.GetComponentInChildren<WeaponFireHurtbox>() is WeaponFireHurtbox hurtbox && hurtbox.Shooter == _hurtBox.Shooter)
            return;

        //The bullet has hit a hitbox
        if (collision.gameObject.GetComponentInParent<Hitbox>())
        {
            //The bullet should destroy itself
            if (DestroyOnHitboxContact)
                Destroy(gameObject);

            return;
        }
    }
}
