using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Controls the player's jump
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Flippable))]
public class JumpController : MonoBehaviour
{
    [Header("Force")]
    [Tooltip("The upwards force that will be applied when jumping")]
    public float JumpForce = 8f;

    [Tooltip("The upwards force that will be when the user lets go of the jump button")]
    public float JumpReleaseForce = 8f;

    [Tooltip("The maximum downwards velocity the player can have")]
    public float MaxFallSpeed = 10f;

    [Header("Game Feel")]
    [Tooltip("The amount of time the player can stay off the ground before they won't be allowed to jump")]
    public float MercyFramesInSeconds = 0.25f;

    [Tooltip("The amount of time before hitting the ground the player can hit the jump button and have it register as a jump")]
    public float BufferedJumpFramesInSeconds = 0.25f;

    /* *** */

    private Rigidbody2D _rigid;
    private Flippable _flippable;

    //The amount of milliseconds the player must have been airborne before mercy frames and buffered inputs will be counted
    private const float DOUBLE_JUMP_PREVENTION_MILLIS = 250f;

    /// <summary>
    /// Gets or sets the last position of the player when they were last grounded
    /// </summary>
    private PositionSnapshot _lastGroundedPosition;

    /// <summary>
    /// Gets or sets the last position of the player when they tried to jump. Whether they failed or not (pressed the jump button)
    /// </summary>
    private PositionSnapshot _lastAttemptedJumpPosition;

    /// <summary>
    /// Gets or sets the last position of the player when they successfully to jump
    /// </summary>
    private PositionSnapshot _lastSuccessfulJumpPosition;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _flippable = GetComponent<Flippable>();

        //Sets up the default ground mask for OnGround2D()
        CommonExtensions.DefaultGroundMask = Commons.Masks.GroundOnly;
    }

    private void Update()
    {
        bool mayJump = MayJump();
        bool onGround = this.OnGround2D();

        /*
         * Update last grounded position
         */
        if (onGround)
            _lastGroundedPosition = PositionSnapshot.FromObject(_flippable);

        DebugScreenDrawer.Enable("onground", "onground: " + onGround);
        DebugScreenDrawer.Enable("mayjump", "mayjump: " + mayJump);

        /*
         * Apply jump
         */
        if (Input.GetButtonDown("Jump"))
        {
            _lastAttemptedJumpPosition = PositionSnapshot.FromObject(_flippable);

            if (mayJump)
            {
                StopAllCoroutines();
                StartCoroutine(CoJump());

                CaptureSuccessfulJumpSnapshot();
            }

        }

        //If the player has buffered a jump (in the last 'BufferedJumpFramesInSeconds' seconds) and is grounded
        else if (onGround && (DateTime.Now - _lastAttemptedJumpPosition.Time).TotalSeconds <= BufferedJumpFramesInSeconds)
        {
            //If the player didn't just leave the ground in a jump (Don't want double jumps)
            if ((DateTime.Now - _lastSuccessfulJumpPosition.Time).TotalMilliseconds > DOUBLE_JUMP_PREVENTION_MILLIS)
            {
                StopAllCoroutines();
                StartCoroutine(CoJump());

                CaptureSuccessfulJumpSnapshot();
            }
        }

        /*
         * Limit falling speed
         */
        float maxFallSpeed = MaxFallSpeed;

        if (Commons.CurrentRoomEffects.HasFlag(RoomEffects.LowGravity))
            maxFallSpeed *= Commons.RoomEffectController.LowGravityMultiplier;

        _rigid.SetVelocityY(currentY => Math.Max(-maxFallSpeed, currentY));
    }

    /// <summary>
    /// Can the player jump at this time?
    /// </summary>
    /// <returns></returns> 
    public bool MayJump()
    {
        if (Cheats.MoonJump)
            return true;

        //The player has mercy frames
        if ((DateTime.Now - _lastGroundedPosition.Time).TotalSeconds <= MercyFramesInSeconds)
        {
            //Do not give mercy frames if the player just jumped
            if ((DateTime.Now - _lastSuccessfulJumpPosition.Time).TotalMilliseconds < DOUBLE_JUMP_PREVENTION_MILLIS)
                return false;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Coroutine: Causes the player to jump dynamically
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoJump()
    {
        //The amount of real-time seconds that have passed since the start of the jump
        float seconds = 0f;

        //Set the initial jump velocity
        _rigid.SetVelocityY(JumpForce);

        //Switches solidity of JumpSwitchBlocks
        foreach (var i in FindObjectsOfType<JumpSwitchBase>())
            i.OnJump();

        //As long as the player can still hold the button
        while (seconds < 0.15f || Cheats.MoonJump)
        {
            //The player is still holding the jump button, keep adding force
            if (Input.GetButton("Jump"))
            {
                //The player has hit a ceiling. Stop
                if (_rigid.velocity.y < JumpForce / 2f && !Cheats.MoonJump)
                {
                    yield break;
                }

                //Keep the players jump
                _rigid.SetVelocityY(JumpForce);
            }

            //The player has let go of the jump button
            else
            {
                if (_rigid.velocity.y > JumpReleaseForce)
                    _rigid.SetVelocityY(JumpReleaseForce);

                yield break;
            }
                

            seconds += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    
    public void CaptureSuccessfulJumpSnapshot()
    {
        _lastSuccessfulJumpPosition = PositionSnapshot.FromObject(_flippable);
        _lastAttemptedJumpPosition = _lastSuccessfulJumpPosition;
    }
}
