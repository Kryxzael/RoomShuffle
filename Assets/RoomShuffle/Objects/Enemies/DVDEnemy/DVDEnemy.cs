using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Flippable), typeof(Rigidbody2D), typeof(Collider2D))]
public class DVDEnemy : MonoBehaviour
{
    [Tooltip("How fast the enemy will fly")]
    public float FlySpeed;
    
    [Tooltip("The angle the enemy will fly in")] [Range(0,90)]
    public float Degree;


    private Rigidbody2D _rigidbody;
    private Flippable _flippable;
    private Collider2D _collider;
    private Vector2 _direction;

    private void Start()
    {
        _flippable = GetComponent<Flippable>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        
        //Creates a vector from the "Degree" variable
        _direction = new Vector2((float)Math.Cos((Math.PI / 180) * Degree), (float)Math.Sin((Math.PI / 180) * Degree));

        //Give the enemy velocity in the direction it is pointing and random velocity vertically
        _rigidbody.velocity = new Vector2(_flippable.DirectionSign , Random.Range(-1,1)).normalized * FlySpeed;
    }

    private void Update()
    {
        //make sure velocity is never zero in any direction ()
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
        Vector2 extents = bounds.extents;
        int invertX = 1;
        int invertY = 1;

        //Raycasts for checking is a collision has been made in the direction it has momentum
        if (_rigidbody.velocity.x > 0)
        {
            RaycastHit2D rightUp = Physics2D.Raycast(new Vector2(bounds.center.x + extents.x, bounds.center.y + extents.y), Vector2.right, 0.03f);
            RaycastHit2D rightDown = Physics2D.Raycast(new Vector2(bounds.center.x + extents.x, bounds.center.y - extents.y), Vector2.right, 0.03f);

            if (rightUp || rightDown)
                invertX = -1;
        }
        else if (_rigidbody.velocity.x < 0)
        {
            RaycastHit2D leftUp = Physics2D.Raycast(new Vector2(bounds.center.x - extents.x, bounds.center.y + extents.y), -Vector2.right, 0.03f);
            RaycastHit2D leftDown = Physics2D.Raycast(new Vector2(bounds.center.x - extents.x, bounds.center.y - extents.y), -Vector2.right, 0.03f);
            
            if (leftDown || leftUp)
                invertX = -1;
        }
        
        if (_rigidbody.velocity.y > 0)
        {
            RaycastHit2D upLeft = Physics2D.Raycast(new Vector2(bounds.center.x - extents.x, bounds.center.y + extents.y), Vector2.up, 0.03f);
            RaycastHit2D upRight = Physics2D.Raycast(new Vector2(bounds.center.x + extents.x, bounds.center.y + extents.y), Vector2.up, 0.03f);
            
            if (upLeft || upRight)
                invertY = -1;
        }
        else if (_rigidbody.velocity.y < 0)
        {
            RaycastHit2D downLeft = Physics2D.Raycast(new Vector2(bounds.center.x - extents.x, bounds.center.y - extents.y), -Vector2.up, 0.03f);
            RaycastHit2D downRight = Physics2D.Raycast(new Vector2(bounds.center.x + extents.x, bounds.center.y - extents.y), -Vector2.up, 0.03f);
            
            if (downLeft || downRight)
                invertY = -1;
        }
        
        //If any horizontal raycasts collides - invert horizontal speed 
        _rigidbody.SetVelocityX(_rigidbody.velocity.x * invertX);
        
        //If any vertical raycasts collides - invert vertical speed 
        _rigidbody.SetVelocityY(_rigidbody.velocity.y * invertY);

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
