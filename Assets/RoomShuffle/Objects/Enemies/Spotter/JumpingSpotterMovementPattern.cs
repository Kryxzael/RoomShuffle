using System;
using UnityEngine;

using Random = UnityEngine.Random;

/// <summary>
/// An enemy pattern that can spot the player and chase them while jumping
/// </summary>
[RequireComponent(typeof(LimitedMovementRadius), typeof(SpriteAnimator))]
public class JumpingSpotterMovementPattern : EnemyScript
{

    [Tooltip("The angle the enemy will jump in")] [Range(0,90)]
    public float Degree;
    
    [Tooltip("The force that the enemy will put into its jump")] [Range(0,90)]
    public float JumpForce;

    [Tooltip("The amount of jumps the jumping spotter will do before choosing a new direction")]
    public RandomValueBetween FumbleJumps = new RandomValueBetween(1f, 3f);
    
    [Tooltip("The amount of time the enemy will be grounded between each jump")]
    public RandomValueBetween GroundTime = new RandomValueBetween(1f, 3f);
    
    [Tooltip("The amount of if time in seconds the enemy will attempt to go home after loosing sight of the player")]
    public float GoHomeTime;

    private bool _lastAnimationWas1;

    [Header("Animations")]
    public SpriteAnimation IdleAnimation;
    public SpriteAnimation JumpAnimation;
    public SpriteAnimation SpotAnimation;
    public SpriteAnimation ChaseAnimation1;
    public SpriteAnimation ChaseAnimation2;

    //The remaining ground time
    private float _groundTimeLeft;

    //how much time is left of the GoHomeTime
    private float _goHomeTimeLeft;
    
    //Is the player on the ground
    private bool grounded;
    
    //how many fumble jumps are left
    private int _fumbleJumpsLeft;
    
    //at which time while grounded the enemy will potentially flip
    private float _flipTime;

    /* *** */

    private Vector2 _direction;
    private SpotPlayer _spotPlayer; 
    private LimitedMovementRadius _limitedMovementRadius;
    private SpriteAnimator _animator;

    void Start()
    {
        _spotPlayer = GetComponent<SpotPlayer>();
        _limitedMovementRadius = GetComponent<LimitedMovementRadius>();
        _animator = GetComponent<SpriteAnimator>();
        
        //Creates a vector from the "Degree" variable. This will be the jump angle
        _direction = new Vector2(
            x: Mathf.Cos((Mathf.PI / 180) * Degree),
            y: Mathf.Sin((Mathf.PI / 180) * Degree)
        ).normalized;
        
    }
    
    void Update()
    {        
        StopOnGround();

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
                break;

            case SpotterPlayerRelationship.Spotted:
                _animator.Animation = SpotAnimation;
                FlipToPlayer();
                break;

            case SpotterPlayerRelationship.Puzzled:
                _animator.Animation = IdleAnimation;
                break;
            
            case SpotterPlayerRelationship.BlindChasing:
                FlipToVector2(transform.Position2D() + _spotPlayer.BlindChaseDirection);
                JumpToSign(Math.Sign(_spotPlayer.BlindChaseDirection.x));
                break;

            case SpotterPlayerRelationship.Chasing:
                FlipToPlayer();
                JumpToSign(Enemy.Flippable.DirectionSign);
                break;
        }
    }

    /// <summary>
    /// Flips the object around to face the player
    /// </summary>
    private void FlipToPlayer()
    {
        FlipToVector2(this.GetPlayer().transform.position);
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
    /// Makes the object jump around randomly
    /// </summary>
    private void FumbleAround()
    {
        //all decisions are made while grounded. Return if not.
        if (!grounded)
        {
            return;
        }

        //if the enemy has waited enough
        if (_groundTimeLeft <= 0f)
        {
            if (_fumbleJumpsLeft > 0)
            {
                //jump the direction the enemy is facing
                JumpToSign(Enemy.Flippable.DirectionSign);
            }
            else
            {
                //resets all counters
                _groundTimeLeft = Commons.GetEffectValue(GroundTime.Pick(), EffectValueType.EnemyWaitTime);
                _fumbleJumpsLeft = FumbleJumps.PickInt() + 1;
                _flipTime = _groundTimeLeft / 2;
            }
        }
        //wait more
        else
        {
            _groundTimeLeft -= Time.deltaTime;

            //halfway through the waiting: 50/50 chance of changing direction
            if (_groundTimeLeft < _flipTime)
            {
                if (Random.Range(0, 2) == 0)
                    Enemy.Flippable.Flip();

                _flipTime = -5;
            }
        }
    }

    /// <summary>
    /// Sets the vertical momentum to 0 if the player is on the ground and don't have momentum upwards.
    /// Updates the grounded boolean
    /// </summary>
    private void StopOnGround()
    {
        //If you have any vertical speed: return
        if (Enemy.Rigidbody.velocity.y > 0.1f)
        {
            grounded = false;
            return;
        }

        //raycast to check if the enemy is near the ground
        var bounds = Enemy.Collider.bounds;
        RaycastHit2D downLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.down, 0.05f);
        RaycastHit2D downRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.down, 0.05f);

        //if the enemy is near ground: stop horizontal speed
        if (downLeft || downRight)
        {
            Enemy.Rigidbody.SetVelocityX(0f);
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    /// <summary>
    /// If grounded, the enemy will to a jump in the horizontal direction decided by the argument
    /// </summary>
    /// <param name="sign"></param>
    private void JumpToSign(int sign)
    {
        sign = Math.Sign(sign);

        if (!grounded || sign == 0)
            return;

        Enemy.Rigidbody.velocity = _direction * Commons.GetEffectValue(JumpForce, EffectValueType.EnemySpeed);
        Enemy.Rigidbody.SetVelocityX(Enemy.Rigidbody.velocity.x * sign);

        _fumbleJumpsLeft--;

        if (_spotPlayer.InPursuit)
        {
            if (_lastAnimationWas1)
                _animator.Animation = ChaseAnimation2;

            else
                _animator.Animation = ChaseAnimation1;

            _lastAnimationWas1 = !_lastAnimationWas1;
        }
            

        else
            _animator.Animation = JumpAnimation;

        _animator.RestartAnimation();
    }

    /// <summary>
    /// The enemy will try to JumpToSign() to its home position. If it uses to long time, the homeposition is updated
    /// </summary>
    private void TryToGoHome()
    {
        //countdown
        _goHomeTimeLeft -= Time.deltaTime;
        
        //if countdown has ended: set new home
        if (_goHomeTimeLeft <= 0)
        {
            _limitedMovementRadius.Home = transform.position;
            _goHomeTimeLeft = Commons.GetEffectValue(GoHomeTime, EffectValueType.EnemyWaitTime);
        }
        //else jump home
        else
        {
            JumpToSign(Enemy.Flippable.DirectionSign);
        }
    }
}