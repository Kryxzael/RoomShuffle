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
[RequireComponent(typeof(LimitedMovementRadius), typeof(FlyingSpotterMovementPattern))]
public class FlyingSpotterMovementPattern : EnemyScript
{
    //The remaining fumble wait time
    private float _fumbleCurrentWaitTime;

    //The remaning time the enemy will attempt to go into the home zone
    private float _goHomeCurrentWaitTime;

    //If the current fumble mode is flying
    private bool _fumbleFly = true;

    /* *** */

    private RenewableLazy<GameObject> _player = new RenewableLazy<GameObject>(() => CommonExtensions.GetPlayer());
    private SpotPlayer _spotPlayer;
    private SpriteAnimator _animator;
    private Vector2 _flyDirection;
    private LimitedMovementRadius _limitedMovementRadius;

    /* *** */

    [Tooltip("The speed the spotter will fly at when fumbling")]
    public float FlyingSpeed;

    [Tooltip("The speed the spotter will fly at when chasing")]
    public float ChaseSpeed;

    [Tooltip("The amount of time the spotter can move or wait before choosing a new action when fumbling")]
    public RandomValueBetween FumbleWaitTime = new RandomValueBetween(3f, 7f);

    [Tooltip("The amount of time the enemy will use to try to get to the home zone")]
    public float GoHomeWaitTime;

    [Header("Animations")]
    public SpriteAnimation IdleAnimation;
    public SpriteAnimation FlyAnimation;
    public SpriteAnimation SpotAnimation;
    public SpriteAnimation ChaseAnimation;

    private void Start()
    {
        _spotPlayer = GetComponent<SpotPlayer>();
        _animator = GetComponent<SpriteAnimator>();
        _limitedMovementRadius = GetComponent<LimitedMovementRadius>();
    }
    
    private void Update()
    {
        switch (_spotPlayer.State)
        {
            case SpotterPlayerRelationship.OutOfRadius:
            case SpotterPlayerRelationship.HiddenInRadius:

                if (!_limitedMovementRadius.InHomeRadius)
                {
                    _flyDirection = (_limitedMovementRadius.Home - (Vector2) transform.position).normalized;
                    TryToGoHome();
                }
                else
                {
                    _goHomeCurrentWaitTime = Commons.GetEffectValue(GoHomeWaitTime, EffectValueType.EnemyWaitTime);
                    FumbleAround();
                }

                if (Mathf.Approximately(Enemy.Rigidbody.velocity.x, 0f))
                    _animator.Animation = IdleAnimation;
                else
                    _animator.Animation = FlyAnimation;
                break;

            case SpotterPlayerRelationship.Spotted:
                FlipToPlayer();
                Enemy.Rigidbody.velocity = default;
                _animator.Animation = SpotAnimation;
                break;

            case SpotterPlayerRelationship.Puzzled:
                Enemy.Rigidbody.velocity = default;
                _animator.Animation = IdleAnimation;
                break;
            
            case SpotterPlayerRelationship.BlindChasing:
                
                _goHomeCurrentWaitTime = Commons.GetEffectValue(GoHomeWaitTime, EffectValueType.EnemyWaitTime);
                Enemy.Rigidbody.velocity = _spotPlayer.BlindChaseDirection * Commons.GetEffectValue(ChaseSpeed, EffectValueType.EnemySpeed);
                
                //Flip the sprite to face the last place the player was
                if (_spotPlayer.BlindChaseDirection.x <= 0)
                {
                    Enemy.Flippable.Direction = Direction1D.Left;
                }
                else
                {
                    Enemy.Flippable.Direction = Direction1D.Right;
                }

                _animator.Animation = ChaseAnimation;

                break;

            case SpotterPlayerRelationship.Chasing:
                FlipToPlayer();
                _goHomeCurrentWaitTime = GoHomeWaitTime;
                Enemy.Rigidbody.velocity = (_player.Value.transform.position - transform.position).normalized * Commons.GetEffectValue(ChaseSpeed, EffectValueType.EnemySpeed);
                _animator.Animation = ChaseAnimation;
                break;
            
        }
        
    }



    /// <summary>
    /// Flips the object around to face the player
    /// </summary>
    private void FlipToPlayer()
    {
        FlipToVector2(_player.Value.transform.position);
    }
    
    /// <summary>
    /// Flips the object around to face a relative direction
    /// </summary>
    private void FlipToVector2(Vector2 position)
    {
        int relativePositionSign = Math.Sign(position.x - transform.position.x);

        if (Enemy.Flippable.DirectionSign != relativePositionSign)
            Enemy.Flippable.Flip();
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
            if (Math.Sign(_flyDirection.x) != Enemy.Flippable.DirectionSign)
            {
                Enemy.Flippable.Flip();
            }

            Enemy.Rigidbody.velocity = _flyDirection * Commons.GetEffectValue(FlyingSpeed, EffectValueType.EnemySpeed);
        }

        //Stationary
        else
        {
            Enemy.Rigidbody.velocity = default;
        }

        //Choose a new action
        if (_fumbleCurrentWaitTime <= 0)
        {
            _flyDirection = Random.insideUnitCircle.normalized;
            _fumbleFly = !_fumbleFly;
            _fumbleCurrentWaitTime = Commons.GetEffectValue(FumbleWaitTime.Pick(), EffectValueType.EnemyWaitTime);
        }
    }

    private void TryToGoHome()
    {

        _goHomeCurrentWaitTime -= Time.deltaTime;
        if (_goHomeCurrentWaitTime <= 0)
        {
            _limitedMovementRadius.Home = transform.position;
            _goHomeCurrentWaitTime = GoHomeWaitTime;
        }
        else
        {
            Enemy.Rigidbody.velocity = (_limitedMovementRadius.Home - (Vector2)transform.position).normalized * Commons.GetEffectValue(FlyingSpeed, EffectValueType.EnemySpeed);
        }
    }
}