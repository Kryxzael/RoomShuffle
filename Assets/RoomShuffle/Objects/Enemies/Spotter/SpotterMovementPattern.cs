using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

/// <summary>
/// An enemy pattern that can spot the player and chase them
/// </summary>
[RequireComponent(typeof(LimitedMovementRadius), typeof(SpriteAnimator))]
public class SpotterMovementPattern : EnemyScript
{
    [Tooltip("The speed the spotter will move at when fumbling")]
    public float WalkSpeed;

    [Tooltip("The speed the spotter will move at when chasing")]
    public float RunSpeed;

    [Tooltip("The amount of time the spotter can move or wait before choosing a new action when fumbling")]
    public RandomValueBetween FumbleWaitTime = new RandomValueBetween(3f, 7f);
    
    [Tooltip("The amount of the in seconds the enemy will attempt to go home after loosing sight of the player")]
    public float GoHomeTime;

    [Header("Animations")]
    public SpriteAnimation IdleAnimation;
    public SpriteAnimation WalkAnimation;
    public SpriteAnimation SpotAnimation;
    public SpriteAnimation ChaseAnimation;

    //how much time is left of the GoHomeTime
    private float _goHomeTimeLeft;
    
    //The remaining fumble wait time
    private float _fumbleCurrentWaitTime;

    //If the current fumble mode is walking
    private bool _fumbleWalk = true;

    /* *** */

    private RenewableLazy<GameObject> _player = new RenewableLazy<GameObject>(() => CommonExtensions.GetPlayer());
    private SpotPlayer _spotPlayer;
    private LimitedMovementRadius _limitedMovementRadius;
    private SpriteAnimator _animator;

    void Start()
    {
        _spotPlayer = GetComponent<SpotPlayer>();
        _limitedMovementRadius = GetComponent<LimitedMovementRadius>();
        _animator = GetComponent<SpriteAnimator>();
    }
    
    void Update()
    {
        switch (_spotPlayer.State)
        {
            case SpotterPlayerRelationship.OutOfRadius:
            case SpotterPlayerRelationship.HiddenInRadius:   
                if (!_limitedMovementRadius.InHomeRadius)
                {
                    TryToGoHome();
                }
                else
                {
                    FumbleAround();
                }

                if (Mathf.Approximately(Enemy.Rigidbody.velocity.x, 0f))
                    _animator.Animation = IdleAnimation;
                else
                    _animator.Animation = WalkAnimation;
                break;

            case SpotterPlayerRelationship.Spotted:
                FlipToPlayer();
                Enemy.Rigidbody.SetVelocityX(0f);
                _animator.Animation = SpotAnimation;
                break;

            case SpotterPlayerRelationship.Puzzled:
                Enemy.Rigidbody.SetVelocityX(0f);
                _animator.Animation = IdleAnimation;
                break;

            case SpotterPlayerRelationship.BlindChasing:
                Enemy.Rigidbody.SetVelocityX(Math.Sign(_spotPlayer.BlindChaseDirection.x) * Commons.GetEffectValue(RunSpeed, EffectValueType.EnemySpeed));
                FlipToVector2(transform.Position2D() + _spotPlayer.BlindChaseDirection);
                _animator.Animation = ChaseAnimation;
                break;

            case SpotterPlayerRelationship.Chasing:
                FlipToPlayer();
                Enemy.Rigidbody.SetVelocityX(Enemy.Flippable.DirectionSign * Commons.GetEffectValue(RunSpeed, EffectValueType.EnemySpeed));
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
        if (_fumbleWalk)
        { 
            Enemy.Rigidbody.SetVelocityX(Enemy.Flippable.DirectionSign * Commons.GetEffectValue(WalkSpeed, EffectValueType.EnemySpeed));
        }

        //Stationary
        else
        {
            Enemy.Rigidbody.SetVelocityX(0f);
        }

        //Choose a new action
        if (_fumbleCurrentWaitTime <= 0)
        {
            if (!_fumbleWalk && Random.Range(0, 2) == 0)
                Enemy.Flippable.Flip();

            _fumbleWalk = !_fumbleWalk;
            _fumbleCurrentWaitTime = Commons.GetEffectValue(FumbleWaitTime.Pick(), EffectValueType.EnemyWaitTime);
        }
    }
    
    private void TryToGoHome()
    {
        _goHomeTimeLeft -= Time.deltaTime;
        if (_goHomeTimeLeft <= 0)
        {
            _limitedMovementRadius.Home = transform.position;
            _goHomeTimeLeft = Commons.GetEffectValue(GoHomeTime, EffectValueType.EnemyWaitTime);
        }
        else
        {
            FlipToVector2(_limitedMovementRadius.Home);
            Enemy.Rigidbody.SetVelocityX(Enemy.Flippable.DirectionSign * Commons.GetEffectValue(WalkSpeed, EffectValueType.EnemySpeed));
        }
    }
}