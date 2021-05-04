using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// An enemy (or object9 that bounces of walls
/// </summary>
public class DVDEnemy : EnemyScript
{
    //The current direction the enemy is traveling in
    private Vector2 _direction;

    /* *** */

    [Tooltip("How fast the enemy will fly")]
    public float FlySpeed;
    
    [Tooltip("The angle the enemy will fly in")] [Range(0,90)]
    public float Degree;


    private void Start()
    {
        //Creates a vector from the "Degree" variable
        _direction = new Vector2(
            x: Mathf.Cos((Mathf.PI / 180) * Degree), 
            y: Mathf.Sin((Mathf.PI / 180) * Degree)
        );

        //Give the enemy velocity in the direction it is pointing and random velocity vertically (up or down)
        Enemy.Rigidbody.velocity = new Vector2(Enemy.Flippable.DirectionSign, Random.Range(0, 2) == 0 ? 1 : -1).normalized * FlySpeed;
    }

    private void Update()
    {
        //make sure velocity is never zero on any axis
        if (Enemy.Rigidbody.velocity.x == 0)
        {
            Enemy.Rigidbody.SetVelocityX(1);
        }
        if (Enemy.Rigidbody.velocity.y == 0)
        {
            Enemy.Rigidbody.SetVelocityY(1);
        }

        //Set the right direction for the enemy
        Enemy.Rigidbody.SetVelocityX(Math.Sign(Enemy.Rigidbody.velocity.x) * _direction.x);
        Enemy.Rigidbody.SetVelocityY(Math.Sign(Enemy.Rigidbody.velocity.y) * _direction.y);

        //set the right speed for the enemy
        Enemy.Rigidbody.velocity = Enemy.Rigidbody.velocity.normalized * Commons.GetEffectValue(FlySpeed, EffectValueType.EnemySpeed);

        Bounds bounds = Enemy.Collider.bounds;

        bool flipX = false;
        bool flipY = false;

        /* 
         * Raycasts for checking is a collision has been made in the direction it has momentum
         */

        const float RAYCAST_DISTANCE = 0.03f;

        //Checks right sides
        if (Enemy.Rigidbody.velocity.x > 0)
        {
            RaycastHit2D rightUp = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.max.y), Vector2.right, RAYCAST_DISTANCE, Commons.Masks.GroundAndBlockers);
            RaycastHit2D rightDown = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.right, RAYCAST_DISTANCE, Commons.Masks.GroundAndBlockers);

            if (rightUp || rightDown)
                flipX = true;
        }

        //Checks left sides
        else if (Enemy.Rigidbody.velocity.x < 0)
        {
            RaycastHit2D leftUp = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.max.y), Vector2.left, RAYCAST_DISTANCE, Commons.Masks.GroundAndBlockers);
            RaycastHit2D leftDown = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.left, RAYCAST_DISTANCE, Commons.Masks.GroundAndBlockers);
            
            if (leftDown || leftUp)
                flipX = true;
        }

        //Checks top
        if (Enemy.Rigidbody.velocity.y > 0)
        {
            RaycastHit2D upLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.max.y), Vector2.up, RAYCAST_DISTANCE, Commons.Masks.GroundAndBlockers);
            RaycastHit2D upRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.max.y), Vector2.up, RAYCAST_DISTANCE, Commons.Masks.GroundAndBlockers);
            
            if (upLeft || upRight)
                flipY = true;
        }

        //Checks bottom
        else if (Enemy.Rigidbody.velocity.y < 0)
        {
            RaycastHit2D downLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.down, RAYCAST_DISTANCE, Commons.Masks.GroundAndBlockers);
            RaycastHit2D downRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.down, RAYCAST_DISTANCE, Commons.Masks.GroundAndBlockers);
            
            if (downLeft || downRight)
                flipY = true;
        }

        //If any horizontal raycasts collides - invert horizontal speed 
        Enemy.Rigidbody.SetVelocityX(Enemy.Rigidbody.velocity.x * (flipX ? -1 : 1));

        //If any vertical raycasts collides - invert vertical speed 
        Enemy.Rigidbody.SetVelocityY(Enemy.Rigidbody.velocity.y * (flipY ? -1 : 1));

        FlipToVelocityDirection();
    }

    /// <summary>
    /// Flips the sprite to face the direction it has momentum
    /// </summary>
    private void FlipToVelocityDirection()
    {
        if (Enemy.Flippable.DirectionSign != Math.Sign(Enemy.Rigidbody.velocity.x) &&
            !(Math.Abs(Enemy.Rigidbody.velocity.x) < 0.001f))
        {
            Enemy.Flippable.Flip();
        }
    }
}
