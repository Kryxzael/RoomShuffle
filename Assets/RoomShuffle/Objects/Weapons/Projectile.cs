using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(WeaponFireHurtbox))]

public abstract class Projectile : MonoBehaviour
{

    private WeaponInstance _weaponInstance;
    private Vector3 _projectileSpwanPoint;
        
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

    protected virtual void Update()
    {
        if (Vector2.Distance(transform.position, _projectileSpwanPoint) > _weaponInstance.Range)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Start()
    {
        WeaponFireHurtbox hurtBox = GetComponent<WeaponFireHurtbox>();
        _weaponInstance = hurtBox.Weapon;
        _projectileSpwanPoint = hurtBox.Shooter.GetProjectilesSpawnPoint();
    }

    public float Speed;


    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Destroy(gameObject);
    // }
}
