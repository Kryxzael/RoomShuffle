using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// An enemy pattern that can spot the player and chase them
/// </summary>
public class StickyBoy : EnemyScript
{
    [Tooltip("The speed the stickyboy will move at")]
    public float WalkSpeed;

    [Tooltip("The direction the enemy will rotate and move in")]
    public bool ClockwiseRotation;

    /* *** */

    private int _enemyDirection;
    private float _freeFall;
    private bool _grounded;
    private bool rotating;

    //The time the enemy will try to connect to the ground after going around a corner
    private const float CONNECT_TO_GROUND_TIME = 0.07f;

    void Start()
    {   
        //Make sure the enemy is facing the right direction. This never changes again because it rotates.
        if (!ClockwiseRotation)
        {
            GetComponent<Flippable>().Flip();
        }

        //the enemy is falling by default
        _freeFall = 100;
        
        _enemyDirection = ClockwiseRotation ? 1 : 3;
    }
    
    void Update()
    {
        //if the stickyboy is in rotating motion. return;
        if (rotating)
            return;
        

        float walkSpeed = Commons.GetEffectValue(WalkSpeed, EffectValueType.EnemySpeed);

        //Make sure the directions is between 1 and 4
        if (_enemyDirection == 5)
            _enemyDirection = 1;
        else if (_enemyDirection == 0)
            _enemyDirection = 4;
        
        // check if enemy is on relative ground
        if (CheckCollision(ClockwiseRotation ? _enemyDirection+1 : _enemyDirection-1))
        {
            _freeFall = 0;
            _grounded = true;
            Enemy.Rigidbody.gravityScale = 0f;

            //Walk relatively forward + towards the relative ground
            Enemy.Rigidbody.velocity = _enemyDirection switch
            {
                1 => Vector2.right * walkSpeed + (ClockwiseRotation ? Vector2.down : Vector2.up),
                2 => Vector2.down * walkSpeed + (ClockwiseRotation ? Vector2.left : Vector2.right),
                3 => Vector2.left * walkSpeed + (ClockwiseRotation ? Vector2.up : Vector2.down),
                4 => Vector2.up * walkSpeed + (ClockwiseRotation ? Vector2.right : Vector2.left),
                _ => throw new InvalidOperationException(),
            };

            // If enemy crash into wall
            if (CheckCollision(_enemyDirection))
            {
                if (ClockwiseRotation)
                {
                    _enemyDirection--;
                    StartCoroutine(RotateDegrees(-1));
                }
                else
                {
                    _enemyDirection++;
                    StartCoroutine(RotateDegrees(1));
                }

            }
        }
        //Enemy not on relative ground
        else
        {
            //The enemy is on the edge of a platform (Rounding a corner)
            if (_freeFall <= CONNECT_TO_GROUND_TIME)
            {
                _freeFall += Time.deltaTime;
                Enemy.Rigidbody.velocity = Vector2.zero;
                
                //Rotates the enemy only once
                if (_grounded)
                {
                    _grounded = false;
                    if (!ClockwiseRotation)
                    {
                        _enemyDirection--;
                        StartCoroutine(RotateDegrees(-1));
                    }
                    else
                    {
                        _enemyDirection++;
                        StartCoroutine(RotateDegrees(1));
                    }
                }

                //Walk in the direction the enemy is facing
                Enemy.Rigidbody.velocity = _enemyDirection switch
                {
                    0 => Vector2.up * walkSpeed,
                    1 => Vector2.right * walkSpeed,
                    2 => Vector2.down * walkSpeed,
                    3 => Vector2.left * walkSpeed,
                    4 => Vector2.up * walkSpeed,
                    5 => Vector2.right * walkSpeed,
                    _ => throw new InvalidOperationException(),
                };
            }

            //Freefalling. Not really supposed to happen normally
            else if (_freeFall > CONNECT_TO_GROUND_TIME)
            {
                transform.eulerAngles = Vector3.zero;
                _freeFall += Time.deltaTime;
                Enemy.Rigidbody.gravityScale = 1f;
                _enemyDirection = ClockwiseRotation ? 1 : 3;
            }
        }

    }

    //Rotates the enemy the number of degrees clockwise
    private IEnumerator RotateDegrees(int degreesTarget)
    {
        rotating = true;

        int degrees = 0;

        Enemy.Collider.enabled = false;

        while (rotating)
        {
            Enemy.Rigidbody.velocity = Vector2.zero;
            transform.eulerAngles += Vector3.forward * (-degreesTarget*2);
            degrees += degreesTarget*2;

            yield return new WaitForSeconds(0.01f);

            if (degrees == degreesTarget * 90)
            {
                rotating = false;
                Enemy.Collider.enabled = true;
                break;
            }
        }
    }

    //Checks if the enemy is very near a solid abject at a given direction
    private bool CheckCollision(int checkDirection)
    {
        float RAYCAST_DISTANCE = 0.2f;
        Bounds bounds = Enemy.Collider.bounds;

        //Keeps the direction values between 1 and 4
        if (checkDirection == 5)
            checkDirection = 1;
        else if (checkDirection == 0)
            checkDirection = 4;
        
        //TODO: Convert to Direction4
        switch (checkDirection)
        {
            case 2: 
                RaycastHit2D downLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.down, RAYCAST_DISTANCE, Commons.Masks.GroundOnly);
                RaycastHit2D downRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.down, RAYCAST_DISTANCE, Commons.Masks.GroundOnly);

                return (downLeft || downRight);

            case 4: 
                RaycastHit2D upLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.max.y), Vector2.up, RAYCAST_DISTANCE, Commons.Masks.GroundOnly);
                RaycastHit2D upRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.max.y), Vector2.up, RAYCAST_DISTANCE, Commons.Masks.GroundOnly);

                return (upLeft || upRight);

            case 3: 
                RaycastHit2D leftUp = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.max.y), Vector2.left, RAYCAST_DISTANCE, Commons.Masks.GroundOnly);
                RaycastHit2D leftDown = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.left, RAYCAST_DISTANCE, Commons.Masks.GroundOnly);

                return (leftUp || leftDown);

            case 1: 
                RaycastHit2D rightUp = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.max.y), Vector2.right, RAYCAST_DISTANCE, Commons.Masks.GroundOnly);
                RaycastHit2D rightDown = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.right, RAYCAST_DISTANCE, Commons.Masks.GroundOnly);

                return (rightUp || rightDown);

            default: 
                return false;

        }
    }
}