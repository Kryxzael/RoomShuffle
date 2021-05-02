using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Controls the player's movement and simulated physics
/// </summary>
public class GroundController : MonoBehaviour
{
    [Header("Acceleration")]
    [Tooltip("The standard acceleration speed when grounded")]
    public float Acceleration = 1500f;

    [Tooltip("The standard acceleration speed when airborne")]
    public float AirAcceleration = 1500f;

    [Header("Max/Min Speeds")]
    [Tooltip("The top speed")]
    public float MaxSpeed = 10f;

    [Header("Max/Min Speeds")]
    [Tooltip("The minimum speed that can be decelerated to when airborne. This number has no effect when the initial speed is below this value")]
    public float AirMinimumSpeed = 2.5f;

    [Header("Deceleration")]
    [Tooltip("The breaking force applied when grounded")]
    public float DecelerationForce = 0.25f;

    [Tooltip("The breaking force applied when airborne")]
    public float AirDecelerationForce = 0.25f;

    [Tooltip("The amount of force to apply when you are on a slope")]
    public float SlopeForce;

    /* *** */

    private Rigidbody2D _rigid;
    private Collider2D _collider;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        /*
         * Calculates the values to use this frame
         */
        RaycastHit2D onGround = this.OnGround2D();

        //The acceleration
        float acc = onGround ? Commons.GetEffectValue(Acceleration, EffectValueType.PlayerAcceleration) : AirAcceleration;

        //The deceleration
        float dec = onGround ? Commons.GetEffectValue(DecelerationForce, EffectValueType.PlayerDeceleration) : AirDecelerationForce;

        //The minimum speed.
        float minSpeed = onGround ? 0f : AirMinimumSpeed;

        if (Mathf.Abs(_rigid.velocity.x) < AirMinimumSpeed)
            minSpeed = 0f; //Sets minimum airspeed to zero if the player is moving lower than the minimum airspeed initially

        /*
         * Input
         */

        float input = Input.GetAxisRaw("Horizontal");

        if (FlipCamera.IsFlipped ^ Commons.CurrentRoomEffects.HasFlag(RoomEffects.ReverseControls))
            input *= -1;

        //Player wants to move right 
        if (input > 0.25f)
        {
            //Player is currently moving in the opposite direction, decelerate them
            if (_rigid.velocity.x < -0.05f)
                Decelerate(dec, 0f);

            //Apply momentum
            else
                _rigid.AddForce(Vector2.right * acc * Time.deltaTime);
        }

        //Player wants to move left
        else if (input < -0.25f)
        {
            //Player is currently moving in the opposite direction, decelerate them
            if (_rigid.velocity.x > 0.05f)
                Decelerate(dec, 0f);

            //Apply momentum
            else
                _rigid.AddForce(Vector2.left * acc * Time.deltaTime);
        }

        //The player is not trying to move, decelerate them
        else
        {
            Decelerate(dec, minSpeed);
        }
    }

    private void FixedUpdate()
    {
        /*
         * Clamp speed
         */

        //Horizontal
        var maxSpeed = Commons.GetEffectValue(MaxSpeed, EffectValueType.PlayerMaxSpeed);
        _rigid.SetVelocityX(currentX => Mathf.Clamp(currentX, -maxSpeed, maxSpeed));
    }

    /// <summary>
    /// Applies a deceleration force
    /// </summary>
    /// <param name="force"></param>
    /// <param name="minimumSpeed"></param>
    private void Decelerate(float force, float minimumSpeed)
    {
        //The player is moving to the right. Apply force to the left
        if (_rigid.velocity.x > 0)
            _rigid.SetVelocityX(currentX => Mathf.Max(minimumSpeed, currentX - force * Time.deltaTime));

        //The player is moving to the left. Apply force to the right
        else if (_rigid.velocity.x < 0)
            _rigid.SetVelocityX(currentX => Mathf.Min(-minimumSpeed, currentX + force * Time.deltaTime));
    }
}
