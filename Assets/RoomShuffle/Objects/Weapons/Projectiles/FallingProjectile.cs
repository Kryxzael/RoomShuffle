using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingProjectile : Projectile
{
    private Rigidbody2D _rigidbody;
    private GameObject _shooter;
    private float _stillTime;
    private const float JUMP_HEIGHT = 12;
    private Flippable _shooterFlippable;
    private float _horizontalSpeed;
    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
        _shooter = GetComponentInChildren<WeaponFireHurtbox>().Shooter.gameObject;
        _shooterFlippable = _shooter.GetComponent<Flippable>();

        //Makes the bullet affected by gravity
        _rigidbody.gravityScale = 1;
        
        //Decide the direction of the projectile
        int direction = Math.Sign(transform.up.x);

        if (direction == 0)
        {
            direction = _shooterFlippable.DirectionSign;
        }

        //Set horizontal speed
        _rigidbody.SetVelocityX(Speed * direction);
        _horizontalSpeed = _rigidbody.velocity.x;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO make this statemant work. Check layer or whatever
        //if the collision is not with ground layer: destroy projectile
        if(false)
        {
            base.OnCollisionEnter2D(collision);
            return;
        }
        
        Vector2 normal = collision.GetContact(0).normal;

        // If the collision is with a wall: destroy object
        if (Math.Abs(normal.x) > 0.2)
        {
            Destroy(gameObject);
            return;
        }

        // If the projectile collides with ground: Set velocity the opposite direction with fixed vertical velocity (JUMP_HEIGHT)
        _rigidbody.SetVelocityY(Math.Sign(collision.relativeVelocity.y) * JUMP_HEIGHT);
    }

    protected override void Update()
    {
        base.Update();

        //Destroy the projectile if the horizontal speed is very low. (It has hit a wall)
        if (Math.Abs(_rigidbody.velocity.x) < Math.Abs(_horizontalSpeed) * 0.3)
        {
            Destroy(gameObject);
            return;
        }

        //Set horizontal speed contantly
        _rigidbody.SetVelocityX(_horizontalSpeed);

        CheckVerticalSpeed();
    }

    /// <summary>
    /// Check if the verical speed is very low. Destroys the projectile if verical speed is low for too long
    /// </summary>
    private void CheckVerticalSpeed()
    {
        const float MAX_TIME_WITH_LOW_SPEED = 0.3f;
        const float LOW_SPEED = 0.1f;
        
        if (Math.Abs(_rigidbody.velocity.y) < LOW_SPEED)
        {
            _stillTime += Time.deltaTime;
        }
        else
        {
            _stillTime = 0;
        }

        if (_stillTime > MAX_TIME_WITH_LOW_SPEED)
        {
            Destroy(gameObject);
        }
    }
}