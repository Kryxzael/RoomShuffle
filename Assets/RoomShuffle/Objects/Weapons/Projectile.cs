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
        }
    }

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
        _hurtBox = GetComponentInChildren<WeaponFireHurtbox>();
        _weaponInstance = _hurtBox.Weapon;
        _projectileSpwanPoint = transform.position;
        
        //Old code for _projectileSpawnPoint beneath
        // _projectileSpwanPoint = hurtBox.Shooter.GetProjectilesSpawnPoint();
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponentInParent<WeaponShooterBase>() == _hurtBox.Shooter)
            return; 
        
        Destroy(gameObject); 
    }
}
