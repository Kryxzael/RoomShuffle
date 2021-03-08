using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

/// <summary>
/// An enemy pattern that can spot the player and chase them
/// </summary>
[RequireComponent(typeof(Flippable), typeof(SpotPlayer), typeof(Rigidbody2D))]
[RequireComponent(typeof(LimitedMovementRadius))]
public class FlyingSpotterMovementPattern : MonoBehaviour
{
    [Tooltip("The speed the spotter will fly at when fumbling")]
    public float FlyingSpeed;

    [Tooltip("The speed the spotter will fly at when chasing")]
    public float ChaseSpeed;

    [Tooltip("The amount of time the spotter can move or wait before choosing a new action when fumbling")]
    public RandomValueBetween FumbleWaitTime = new RandomValueBetween(3f, 7f);
    
    [Tooltip("The amount of time the enemy will use to try to get to the home zone")]
    public float GoHomeWaitTime;

    //The remaining fumble wait time
    private float _fumbleCurrentWaitTime;
    
    //The remaning time the enemy will attempt to go into the home zone
    private float _goHomeCurrentWaitTime;

    //If the current fumble mode is flying
    private bool _fumbleFly = true;

    /* *** */

    private Rigidbody2D _rigid;
    private RenewableLazy<GameObject> _player = new RenewableLazy<GameObject>(() => CommonExtensions.GetPlayer());
    private Flippable _flippable;
    private SpotPlayer _spotPlayer;
    private Vector2 _flyDirection;
    private SpriteRenderer _spriteRenderer;
    private LimitedMovementRadius _limitedMovementRadius;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _flippable = GetComponent<Flippable>();
        _spotPlayer = GetComponent<SpotPlayer>();
        _limitedMovementRadius = GetComponent<LimitedMovementRadius>();
    }
    
    private void Update()
    {
        //TODO Delete this function
        ShowDebugColors();


        switch (_spotPlayer.State)
        {
            case SpotterPlayerRelationship.OutOfRadius:
            case SpotterPlayerRelationship.HiddenInRadius:

                if (!_limitedMovementRadius.InHomeRadius)
                {
                    _flyDirection = (_limitedMovementRadius.Home - (Vector2) transform.position).normalized;
                    tryToGoHome();
                }
                else
                {
                    FumbleAround();
                }
                break;

            case SpotterPlayerRelationship.Spotted:
                FlipToPlayer();
                _rigid.velocity = default;
                break;

            case SpotterPlayerRelationship.Puzzled:
                _rigid.velocity = default;
                break;
            
            case SpotterPlayerRelationship.BlindChasing:
                _goHomeCurrentWaitTime = GoHomeWaitTime;
                _rigid.velocity = _spotPlayer.BlindChaseDirection * ChaseSpeed;
                
                //Flip the sprite to face the last place the player was
                if (_spotPlayer.BlindChaseDirection.x <= 0)
                {
                    _flippable.Direction = Direction1D.Left;
                }
                else
                {
                    _flippable.Direction = Direction1D.Right;
                }

                break;

            case SpotterPlayerRelationship.Chasing:
                FlipToPlayer();
                _goHomeCurrentWaitTime = GoHomeWaitTime;
                _rigid.velocity = (_player.Value.transform.position - transform.position).normalized * ChaseSpeed;
                break;
            
        }
        
    }



    /// <summary>
    /// Flips the object around to face the player
    /// </summary>
    private void FlipToPlayer()
    {
        int relativePositionSign = Math.Sign(_player.Value.transform.position.x - transform.position.x);

        if (_flippable.DirectionSign != relativePositionSign)
            _flippable.Flip();
    }

    /// <summary>
    /// Makes the object move around randomly
    /// </summary>
    private void FumbleAround()
    {
        _fumbleCurrentWaitTime -= Time.deltaTime;

        //Fumbling
        if (_fumbleFly)
        {
            if (Math.Sign(_flyDirection.x) != _flippable.DirectionSign)
            {
                _flippable.Flip();
            }

            _rigid.velocity = _flyDirection * FlyingSpeed;
        }

        //Stationary
        else
        {
            _rigid.velocity = default;
        }

        //Choose a new action
        if (_fumbleCurrentWaitTime <= 0)
        {
            _flyDirection = Random.insideUnitCircle.normalized;
            _fumbleFly = !_fumbleFly;
            _fumbleCurrentWaitTime = FumbleWaitTime.Pick();
        }
    }

    private void tryToGoHome()
    {

        _goHomeCurrentWaitTime -= Time.deltaTime;
        if (_goHomeCurrentWaitTime <= 0)
        {
            _limitedMovementRadius.Home = transform.position;
            _goHomeCurrentWaitTime = GoHomeWaitTime;
        }
        else
        {
            _rigid.velocity = (_limitedMovementRadius.Home - (Vector2)transform.position).normalized * FlyingSpeed;
        }
    }

    private void ShowDebugColors()
    {
        switch (_spotPlayer.State)
        {
            case SpotterPlayerRelationship.OutOfRadius:
                _spriteRenderer.color = Color.green;
                break;
            case SpotterPlayerRelationship.HiddenInRadius:
                _spriteRenderer.color = Color.blue;
                break;
            case SpotterPlayerRelationship.Spotted:
                _spriteRenderer.color = Color.yellow;
                break;
            case SpotterPlayerRelationship.Puzzled:
                _spriteRenderer.color = Color.magenta;
                break;
            case SpotterPlayerRelationship.Chasing:
                _spriteRenderer.color = Color.red;
                break;
            case SpotterPlayerRelationship.BlindChasing:
                _spriteRenderer.color = Color.cyan;
                break;
            default:
                _spriteRenderer.color = Color.black;
                break;
        }
    }
}