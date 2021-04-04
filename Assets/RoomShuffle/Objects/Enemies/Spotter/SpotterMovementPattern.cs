using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

/// <summary>
/// An enemy pattern that can spot the player and chase them
/// </summary>
[RequireComponent(typeof(LimitedMovementRadius))]
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

    void Start()
    {
        _spotPlayer = GetComponent<SpotPlayer>();
        _limitedMovementRadius = GetComponent<LimitedMovementRadius>();
    }
    
    void Update()
    {
        ShowDebugColors(); //TODO Delete this
        
        switch (_spotPlayer.State)
        {
            case SpotterPlayerRelationship.OutOfRadius:
            case SpotterPlayerRelationship.HiddenInRadius:   
                if (!_limitedMovementRadius.InHomeRadius)
                {
                    tryToGoHome();
                }
                else
                {
                    FumbleAround();
                }
                break;

            case SpotterPlayerRelationship.Spotted:
                FlipToPlayer();
                Enemy.Rigidbody.SetVelocityX(0f);
                break;

            case SpotterPlayerRelationship.Puzzled:
                Enemy.Rigidbody.SetVelocityX(0f);
                break;
            
            case SpotterPlayerRelationship.BlindChasing:
                Enemy.Rigidbody.SetVelocityX(Math.Sign(_spotPlayer.BlindChaseDirection.x) * Commons.GetEffectValue(RunSpeed, EffectValueType.EnemySpeed));
                break;

            case SpotterPlayerRelationship.Chasing:
                FlipToPlayer();
                Enemy.Rigidbody.SetVelocityX(Enemy.Flippable.DirectionSign * Commons.GetEffectValue(RunSpeed, EffectValueType.EnemySpeed));
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
    
    private void tryToGoHome()
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
    
    private void ShowDebugColors()
    {
        Enemy.SpriteRenderer.color = _spotPlayer.State switch
        {
            SpotterPlayerRelationship.OutOfRadius => Color.green,
            SpotterPlayerRelationship.HiddenInRadius => Color.blue,
            SpotterPlayerRelationship.Spotted => Color.yellow,
            SpotterPlayerRelationship.Puzzled => Color.magenta,
            SpotterPlayerRelationship.Chasing => Color.red,
            SpotterPlayerRelationship.BlindChasing => Color.cyan,
            _ => Color.black,
        };
    }
}