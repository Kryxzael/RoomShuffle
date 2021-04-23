using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// An enemy pattern for the digging spotter.
/// </summary>
[RequireComponent(typeof(LimitedMovementRadius), typeof(SpriteAnimator))]
public class DiggingSpotter : EnemyScript
{
    [Tooltip("The speed the spotter will move at when digging")]
    public float DigSpeed;

    [Tooltip("The speed the spotter will move at when chasing")]
    public float RunSpeed;

    [Header("Animations")]
    public SpriteAnimation IdleAnimation;
    public SpriteAnimation DigIdleAnimation;
    public SpriteAnimation DigChaseAnimation;
    public SpriteAnimation ChaseAnimation;

    [Tooltip("The force the enemy will jump up from the ground with")]
    public float PopUpJumpVelocity = 18;
    
    //The remaining fumble wait time
    private float _fumbleCurrentWaitTime;

    //If the current fumble mode is walking
    private bool _fumbleWalk = true;

    //If the enemy is above the ground
    private bool _aboveGround = true;

    // if the enemy is either digging down or popping up
    private bool _inTransitionAnimation = false;

    private int _relativePosition = 0;

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

        // start of by digging down
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

                //If the enemy is under ground and has turned (which means the player is directly over the player):
                //pop up from the ground.
                if (FlipToPlayer() && !_aboveGround && (_relativePosition != Math.Sign(_player.Value.transform.position.x - transform.position.x)))
                {
                    StartCoroutine(PopUp());
                }

                //Chase the player above ground
                if (_aboveGround)
                {
                    Enemy.Rigidbody.SetVelocityX(Enemy.Flippable.DirectionSign * Commons.GetEffectValue(RunSpeed, EffectValueType.EnemySpeed));
                    _animator.Animation = ChaseAnimation;
                    break;   
                }
                //Chase the player under ground
                else
                {
                    Enemy.Rigidbody.SetVelocityX(Enemy.Flippable.DirectionSign * Commons.GetEffectValue(DigSpeed, EffectValueType.EnemySpeed));
                    _animator.Animation = DigChaseAnimation;
                    break;
                }
        }
        
        //The vertical position of the player relative to the enemy
        _relativePosition = Math.Sign(_player.Value.transform.position.x - transform.position.x);
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
    /// Makes the object idle under ground
    /// </summary>
    private void Idle()
    {
        if (_aboveGround)
        {
            StartCoroutine(DigDown());
        }

        Enemy.Rigidbody.SetVelocityX(0);
    }

    /// <summary>
    /// Makes the enemy pop up from the ground.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PopUp()
    {
        //How much time the enemy waits before it pops out of the ground
        const float WAIT_TIME_BEFORE_POPPING = 0.15f;
        
        //the enemy is already above ground
        if (_aboveGround)
            yield break;
        
        //TODO telegraph jumping
        yield return new WaitForSeconds(WAIT_TIME_BEFORE_POPPING);

        //Activate hitbox before animation starts
        Enemy.Hitbox.gameObject.SetActive(true);
        
        //deactivate HUD
        Enemy.HUD.SetActive(true);
        
        _inTransitionAnimation = true;
        
        //Use idleanimation as jumping sprite
        _animator.Animation = IdleAnimation;
        
        //Make enemy jump
        Enemy.Rigidbody.SetVelocityY(PopUpJumpVelocity);
        Enemy.Rigidbody.SetVelocityX(0);
        
        //wait for the enemy to hit the ground
        yield return new WaitUntil(() => this.OnGround2D());

        //start chasing
        _animator.Animation = ChaseAnimation;
        _aboveGround = true;
        _inTransitionAnimation = false;
        
    }

    private IEnumerator DigDown()
    {
        //For how long the enemy should wait before it starts digging down again
        const float WAIT_TIME_BEFORE_DIGGING = 1f;
        
        //the enemy is already under ground
        if (!_aboveGround)
            yield break;
        
        Enemy.Rigidbody.SetVelocityX(0);
        _inTransitionAnimation = true;
        _animator.Animation = IdleAnimation;

        //Wait before goinf under ground
        yield return new WaitForSeconds(WAIT_TIME_BEFORE_DIGGING);

        //Start idling under ground
        _animator.Animation = DigIdleAnimation;
        _aboveGround = false;
        
        //Deactivate hitbox
        Enemy.Hitbox.gameObject.SetActive(false);
        
        // Hide Hud
        Enemy.HUD.SetActive(false);
        _inTransitionAnimation = false;
    }
    
}