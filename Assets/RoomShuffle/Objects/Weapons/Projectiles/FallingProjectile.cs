using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingProjectile : Projectile
{
    private Rigidbody2D _rigidbody;
    private GameObject _shooter;

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
        _shooter = GetComponent<WeaponFireHurtbox>().Shooter.gameObject;
        
        //Makes the bullet affected by gravity
        _rigidbody.gravityScale = 1;
        
        //sets the speed in the facing direction
        _rigidbody.velocity = transform.up * Speed;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter(Collision other)
    {
        //Return if the bullets collides with the shooter or another bullet
        if (other.gameObject.tag.Equals(_shooter.tag) || other.gameObject.tag.Equals("Projectile"))
            return;
        
        _rigidbody.velocity = Vector2.zero;
    }
}