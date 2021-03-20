using System;
using UnityEngine;

using Random = UnityEngine.Random;

/// <summary>
/// An enemy pattern that can spot the player and chase them while jumping
/// </summary>
[RequireComponent(typeof(Flippable), typeof(SpotPlayer), typeof(Rigidbody2D))]
[RequireComponent(typeof(LimitedMovementRadius))]
public class JumpingSpotterMovementPattern : MonoBehaviour
{

    [Tooltip("The angle the enemy will jump in")] [Range(0,90)]
    public float Degree;
    
    [Tooltip("The force that the enemy will pu into its jump")] [Range(0,90)]
    public float JumpForce;

    [Tooltip("The amount of jumps the jumping spotter will do before choosing a new direction")]
    public RandomValueBetween FumbleJumps = new RandomValueBetween(1f, 3f);
    
    [Tooltip("The amount of time the enemy will be grounded between each jump")]
    public RandomValueBetween GroundTime = new RandomValueBetween(1f, 3f);
    
    [Tooltip("The amount of if time in seconds the enemy will attempt to go home after loosing sight of the player")]
    public float GoHomeTime;
    
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
    private Rigidbody2D _rigid;
    private Collider2D _collider;
    private GameObject _player;
    private Flippable _flippable;
    private SpotPlayer _spotPlayer; //TODO Delete this line
    private SpriteRenderer _spriteRenderer;
    private LimitedMovementRadius _limitedMovementRadius;

    void Start()
    {
        _player = this.GetPlayer();
        _rigid = GetComponent<Rigidbody2D>();
        _flippable = GetComponent<Flippable>();
        _spotPlayer = GetComponent<SpotPlayer>();
        _spriteRenderer = GetComponent<SpriteRenderer>(); //TODO Delete this line
        _collider = GetComponent<Collider2D>();
        _limitedMovementRadius = GetComponent<LimitedMovementRadius>();
        
        //Creates a vector from the "Degree" variable. This will be the jump angle
        _direction = new Vector2(
            x: Mathf.Cos((Mathf.PI / 180) * Degree),
            y: Mathf.Sin((Mathf.PI / 180) * Degree)
        ).normalized;
        
    }
    
    void Update()
    {
        ShowDebugColors(); //TODO Delete this line
        
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
                FlipToPlayer();
                break;

            case SpotterPlayerRelationship.Puzzled:
                break;
            
            case SpotterPlayerRelationship.BlindChasing:
                FlipToVector2(_spotPlayer.BlindChaseDirection);
                JumpToSign(Math.Sign(_spotPlayer.BlindChaseDirection.x));
                break;

            case SpotterPlayerRelationship.Chasing:
                FlipToPlayer();
                JumpToSign(_flippable.DirectionSign);
                break;
        }
    }

    /// <summary>
    /// Flips the object around to face the player
    /// </summary>
    private void FlipToPlayer()
    {
        FlipToVector2(_player.transform.position);
    }
    
    /// <summary>
    /// Flips the object around to face a relative direction
    /// </summary>
    private void FlipToVector2(Vector2 position)
    {
        int relativePositionSign = Math.Sign(position.x - transform.position.x);

        if (_flippable.DirectionSign != relativePositionSign)
            _flippable.Flip();
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
                JumpToSign(_flippable.DirectionSign);
            }
            else
            {
                //resets all counters
                _groundTimeLeft = GroundTime.Pick();
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
                    _flippable.Flip();

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
        if (_rigid.velocity.y > 0.1f)
        {
            grounded = false;
            return;
        }

        //raycast to check if the enemy is near the ground
        var bounds = _collider.bounds;
        RaycastHit2D downLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), Vector2.down, 0.05f);
        RaycastHit2D downRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.min.y), Vector2.down, 0.05f);

        //if the enemy is near ground: stop horizontal speed
        if (downLeft || downRight)
        {
            _rigid.SetVelocityX(0f);
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    /// <summary>
    /// If grounded, the enemy will to a jump in det horizontal direction decided by the argument
    /// </summary>
    /// <param name="sign"></param>
    private void JumpToSign(int sign)
    {
        sign = Math.Sign(sign);

        if (!grounded || sign == 0)
            return;
        
        _rigid.velocity = _direction * JumpForce;
        _rigid.SetVelocityX(_rigid.velocity.x * sign);

        _fumbleJumpsLeft--;
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
            _goHomeTimeLeft = GoHomeTime;
        }
        //else jump home
        else
        {
            JumpToSign(_flippable.DirectionSign);
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