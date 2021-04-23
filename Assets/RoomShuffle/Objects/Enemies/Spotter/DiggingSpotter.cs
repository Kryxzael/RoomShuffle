using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

/// <summary>
/// An enemy pattern that can spot the player and chase them
/// </summary>
[RequireComponent(typeof(LimitedMovementRadius), typeof(SpriteAnimator))]
public class DiggingSpotter : EnemyScript
{
    [Tooltip("The speed the spotter will move at when digging")]
    public float DigSpeed;

    [Tooltip("The speed the spotter will move at when chasing")]
    public float RunSpeed;

    [Tooltip("The amount of time the spotter can move or wait before choosing a new action when fumbling")]
    public RandomValueBetween FumbleWaitTime = new RandomValueBetween(3f, 7f);

    [Header("Animations")]
    public SpriteAnimation IdleAnimation;
    public SpriteAnimation DigIdleAnimation;
    public SpriteAnimation DigChaseAnimation;
    public SpriteAnimation ChaseAnimation;

    public float PopUpJumpForce = 10;


    //The remaining fumble wait time
    private float _fumbleCurrentWaitTime;

    //If the current fumble mode is walking
    private bool _fumbleWalk = true;

    //If the enemy is above the ground
    private bool _aboveGround = true;

    // if the enemy is either digging down or popping up
    private bool _inTransitionAnimation = false;

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

        StartCoroutine(DigDown());
    }
    
    void Update()
    {
        //if the enemy is in a transition animation return.
        if (_inTransitionAnimation)
        {
            return;
        }

        switch (_spotPlayer.State)
        {
            case SpotterPlayerRelationship.OutOfRadius:
            case SpotterPlayerRelationship.HiddenInRadius:
                Idle();
                break;
            
            case SpotterPlayerRelationship.Spotted:
                
                FlipToPlayer();
                Enemy.Rigidbody.SetVelocityX(0f);
                break;

            case SpotterPlayerRelationship.Puzzled:
                Enemy.Rigidbody.SetVelocityX(0f);
                break;

            case SpotterPlayerRelationship.Chasing:

                if (FlipToPlayer() && !_aboveGround)
                {
                    StartCoroutine(PopUp());
                }

                if (_aboveGround)
                {
                    Enemy.Rigidbody.SetVelocityX(Enemy.Flippable.DirectionSign * Commons.GetEffectValue(RunSpeed, EffectValueType.EnemySpeed));
                    _animator.Animation = ChaseAnimation;
                    break;   
                }
                else
                {
                    Enemy.Rigidbody.SetVelocityX(Enemy.Flippable.DirectionSign * Commons.GetEffectValue(DigSpeed, EffectValueType.EnemySpeed));
                    _animator.Animation = DigChaseAnimation;
                    break;
                }
        }
    }

    /// <summary>
    /// Flips the object around to face the player
    /// Returns true if the enemy was flipped
    /// </summary>
    private bool FlipToPlayer()
    {
        return FlipToVector2(_player.Value.transform.position);
    }
    
    /// <summary>
    /// Flips the object around to face a relative direction.
    /// Returns true if the object was flipped
    /// </summary>
    private bool FlipToVector2(Vector2 position)
    {
        int relativePositionSign = Math.Sign(position.x - transform.position.x);

        if (Enemy.Flippable.DirectionSign != relativePositionSign)
        {
            Enemy.Flippable.Flip();
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Makes the object idle
    /// </summary>
    private void Idle()
    {
        if (_aboveGround)
        {
            StartCoroutine(DigDown());
        }

        Enemy.Rigidbody.SetVelocityX(0);
    }

    private IEnumerator PopUp()
    {
        //the enemy is already above ground
        if (_aboveGround)
            yield break;

        Enemy.Rigidbody.SetVelocityX(0);
        Enemy.Hitbox.gameObject.SetActive(true);
        Enemy.HUD.SetActive(true);
        _inTransitionAnimation = true;
        _animator.Animation = IdleAnimation;
        Enemy.Rigidbody.SetVelocityY(PopUpJumpForce);
        Enemy.Rigidbody.SetVelocityX(0);
        
        yield return new WaitUntil(() => this.OnGround2D());

        _animator.Animation = ChaseAnimation;
        _aboveGround = true;
        _inTransitionAnimation = false;
        
    }

    private IEnumerator DigDown()
    {
        //the enemy is already under ground
        if (!_aboveGround)
            yield break;
        
        Enemy.Rigidbody.SetVelocityX(0);
        _inTransitionAnimation = true;
        _animator.Animation = IdleAnimation;

        yield return new WaitForSeconds(1);

        _animator.Animation = DigIdleAnimation;
        _aboveGround = false;
        Enemy.Hitbox.gameObject.SetActive(false);
        Enemy.HUD.SetActive(false);
        _inTransitionAnimation = false;
    }
    
}