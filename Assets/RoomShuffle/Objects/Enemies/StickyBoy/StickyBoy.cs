using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

/// <summary>
/// An enemy pattern that can spot the player and chase them
/// </summary>
[RequireComponent(  typeof(Rigidbody2D), typeof(Flippable))]
public class StickyBoy : MonoBehaviour
{
    [Tooltip("The speed the stickyboy will move at")]
    public float WalkSpeed;

    [Tooltip("The direction the enemy will rotate and move in")]
    public bool ClockWiseRotation;

    private Collider2D _collider;
    private int _enemyDirection;
    private int _rotation;
    private float _freeFall;
    private bool _grounded;
    private Rigidbody2D _rigid;
    private bool rotating;

    //The time the enemy will try to connect to the ground after going around a corner
    private float _connectToGroundTime = 0.07f;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        
        //Make sure the enemy is facing the right direction. This never changes again because it rotates.
        if (!ClockWiseRotation)
        {
            GetComponent<Flippable>().Flip();
        }

        //the enemy is falling by default
        _freeFall = 100;
        
        _enemyDirection = ClockWiseRotation ? 1 : 3;
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
        if (CheckCollision(ClockWiseRotation ? _enemyDirection+1 : _enemyDirection-1))
        {
            _freeFall = 0;
            _grounded = true;
            _rigid.gravityScale = 0;

            //Walk relatively forward + towards the relative ground
            switch (_enemyDirection)
            {
                case 1: _rigid.velocity = Vector2.right * walkSpeed + (ClockWiseRotation ? Vector2.down : Vector2.up); break;
                case 2: _rigid.velocity = Vector2.down * walkSpeed + (ClockWiseRotation ? Vector2.left : Vector2.right); break;
                case 3: _rigid.velocity = Vector2.left * walkSpeed + (ClockWiseRotation ? Vector2.up : Vector2.down); break;
                case 4: _rigid.velocity = Vector2.up * walkSpeed + (ClockWiseRotation ? Vector2.right : Vector2.left); break;
            }
            
            // If enemy crash into wall
            if (CheckCollision(_enemyDirection))
            {
                if (ClockWiseRotation)
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
            if (_freeFall <= _connectToGroundTime)
            {
                _freeFall += Time.deltaTime;
                _rigid.velocity = Vector2.zero;
                
                //Rotates the enemy only once
                if (_grounded)
                {
                    _grounded = false;
                    if (!ClockWiseRotation)
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
                switch (_enemyDirection)
                {
                    case 0: _rigid.velocity = Vector2.up * walkSpeed; 
                        break;
                    case 1: _rigid.velocity = Vector2.right * walkSpeed; 
                        break;
                    case 2: _rigid.velocity = Vector2.down * walkSpeed; 
                        break;
                    case 3: _rigid.velocity = Vector2.left * walkSpeed; 
                        break;
                    case 4: _rigid.velocity = Vector2.up * walkSpeed; 
                        break;
                    case 5: _rigid.velocity = Vector2.right * walkSpeed; 
                        break;
                }
            }
            //Freefalling. Not really supposed to happen normally
            else if (_freeFall > _connectToGroundTime)
            {
                transform.eulerAngles = Vector3.zero;
                _freeFall += Time.deltaTime;
                _rigid.gravityScale = 1;
                _enemyDirection = ClockWiseRotation ? 1 : 3;
            }
        }
        
    }

    //Rotates the enemy the number of degrees clockwise
    private IEnumerator RotateDegrees(int degreesTarget)
    {
        rotating = true;

        int degrees = 0;

        _collider.enabled = false;

        while (rotating)
        {
            _rigid.velocity = Vector2.zero;
            transform.eulerAngles += Vector3.forward * (-degreesTarget*2);
            degrees += degreesTarget*2;

            yield return new WaitForSeconds(0.01f);

            if (degrees == degreesTarget * 90)
            {
                rotating = false;
                _collider.enabled = true;
                break;
            }
        }
    }

    //Checks if the enemy is very near a solid abject at a given direction
    private bool CheckCollision(int checkDirection)
    {
        float RAYCAST_DISTANCE = 0.2f;
        Bounds bounds = _collider.bounds;

        //Keeps the direction values between 1 and 4
        if (checkDirection == 5)
            checkDirection = 1;
        else if (checkDirection == 0)
            checkDirection = 4;
        
        switch (checkDirection)
        {
            case 2: 
                RaycastHit2D downLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.down, RAYCAST_DISTANCE);
                RaycastHit2D downRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.down, RAYCAST_DISTANCE);

                return (downLeft || downRight);

            case 4: 
                RaycastHit2D upLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.max.y), Vector2.up, RAYCAST_DISTANCE);
                RaycastHit2D upRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.max.y), Vector2.up, RAYCAST_DISTANCE);

                return (upLeft || upRight);

            case 3: 
                RaycastHit2D leftUp = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.max.y), Vector2.left, RAYCAST_DISTANCE);
                RaycastHit2D leftDown = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.left, RAYCAST_DISTANCE);

                return (leftUp || leftDown);

            case 1: 
                RaycastHit2D rightUp = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.max.y), Vector2.right, RAYCAST_DISTANCE);
                RaycastHit2D rightDown = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.right, RAYCAST_DISTANCE);

                return (rightUp || rightDown);

            default: return false;

        }
    }
}