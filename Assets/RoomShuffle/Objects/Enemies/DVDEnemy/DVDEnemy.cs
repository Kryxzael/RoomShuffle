using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Flippable), typeof(Rigidbody2D), typeof(Collider2D))]
public class DVDEnemy : MonoBehaviour
{
    private Vector2 _direction;

    [Tooltip("How fast the enemy will fly")]
    public float FlySpeed;
    
    [Tooltip("The angle the enemy will fly in")] [Range(0,90)]
    public float Degree;

    /* *** */

    private Rigidbody2D _rigidbody;
    private Flippable _flippable;
    private Collider2D _collider;

    private void Start()
    {
        _flippable = GetComponent<Flippable>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        //Creates a vector from the "Degree" variable
        _direction = new Vector2(
            x: Mathf.Cos((Mathf.PI / 180) * Degree), 
            y: Mathf.Sin((Mathf.PI / 180) * Degree)
        );

        //Give the enemy velocity in the direction it is pointing and random velocity vertically (up or down)
        _rigidbody.velocity = new Vector2(_flippable.DirectionSign, Random.Range(0, 2) == 0 ? 1 : -1).normalized * FlySpeed;
    }

    private void Update()
    {
        //make sure velocity is never zero in any direction
        if (_rigidbody.velocity.x == 0)
        {
            _rigidbody.SetVelocityX(1);
        }
        if (_rigidbody.velocity.y == 0)
        {
            _rigidbody.SetVelocityY(1);
        }
        
        //Set the right direction for the enemy
        _rigidbody.SetVelocityX(Math.Sign(_rigidbody.velocity.x) * _direction.x);
        _rigidbody.SetVelocityY(Math.Sign(_rigidbody.velocity.y) * _direction.y);
        
        //set the right speed for the enemy
        _rigidbody.velocity = _rigidbody.velocity.normalized * FlySpeed;

        Bounds bounds = _collider.bounds;

        bool flipX = false;
        bool flipY = false;

        /* 
         * Raycasts for checking is a collision has been made in the direction it has momentum
         */

        const float RAYCAST_DISTANCE = 0.03f;

        //Checks right sides
        if (_rigidbody.velocity.x > 0)
        {
            RaycastHit2D rightUp = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.max.y), Vector2.right, RAYCAST_DISTANCE);
            RaycastHit2D rightDown = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.right, RAYCAST_DISTANCE);

            if (rightUp || rightDown)
                flipX = true;
        }

        //Checks left sides
        else if (_rigidbody.velocity.x < 0)
        {
            RaycastHit2D leftUp = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.max.y), Vector2.left, RAYCAST_DISTANCE);
            RaycastHit2D leftDown = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.left, RAYCAST_DISTANCE);
            
            if (leftDown || leftUp)
                flipX = true;
        }

        //Checks top
        if (_rigidbody.velocity.y > 0)
        {
            RaycastHit2D upLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.max.y), Vector2.up, RAYCAST_DISTANCE);
            RaycastHit2D upRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.max.y), Vector2.up, RAYCAST_DISTANCE);
            
            if (upLeft || upRight)
                flipY = true;
        }

        //Checks bottom
        else if (_rigidbody.velocity.y < 0)
        {
            RaycastHit2D downLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.down, RAYCAST_DISTANCE);
            RaycastHit2D downRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.down, RAYCAST_DISTANCE);
            
            if (downLeft || downRight)
                flipY = true;
        }
        
        //If any horizontal raycasts collides - invert horizontal speed 
        _rigidbody.SetVelocityX(_rigidbody.velocity.x * (flipX ? -1 : 1));
        
        //If any vertical raycasts collides - invert vertical speed 
        _rigidbody.SetVelocityY(_rigidbody.velocity.y * (flipY ? -1 : 1));

        FlipToVelocityDirection();
    }

    /// <summary>
    /// Flips the sprite to face the direction it has momentum
    /// </summary>
    private void FlipToVelocityDirection()
    {
        if (_flippable.DirectionSign != Math.Sign(_rigidbody.velocity.x))
            _flippable.Flip();
    }
}
