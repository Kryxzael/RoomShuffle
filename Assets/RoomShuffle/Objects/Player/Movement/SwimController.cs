using RoomShuffle.Defaults;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Lets the player swim in water
/// </summary>
[RequireComponent(typeof(SpriteAnimator))]
[RequireComponent(typeof(MultiSoundPlayer))]
public class SwimController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public bool _lastSubmergeState;
    public SpriteAnimator _animator;
    public MultiSoundPlayer _multiSoundPlayer;

    /* *** */

    [Tooltip("How quickly the player will ascend when swimming")]
    public float SwimForce = 10;

    [Tooltip("The amount of upwards force that will be applied when the player leaves water upwards")]
    public float JumpOutForce = 30;

    [Header("Limits")]
    [Tooltip("The maximum speed the player can have moving upwards when in water")]
    public float MaxAscensionSpeed = 10;

    [Tooltip("The maximum speed the player can have moving downwards when in water")]
    public float MaxSinkSpeed = 10;

    [Tooltip("The maximum speed the player can have moving sideways when in water")]
    public float MaxHorizontalSpeed = 5;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<SpriteAnimator>();
        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();
    }

    private void Update()
    {
        bool isSubmerged = Water.IsSubmerged(_rigidbody);

        //Let the player jump out of the water properly
        if (!isSubmerged && _lastSubmergeState && Input.GetButton("Jump"))
            _rigidbody.SetVelocityY(JumpOutForce);

        _lastSubmergeState = Water.IsSubmerged(_rigidbody);

        //If not in water, don't run the rest of the script
        if (!isSubmerged)
            return;

        //Swim up
        if (Input.GetButtonDown("Jump"))
        {
            /*
             * If the player is sinking at a high speed they are decelerated so that the sinking effect isn't so strong
             * that the swimming isn't doing anything
             * In other words... game feel
             */

            float clampedSinkSpeedBeforeSwim = MaxSinkSpeed / 4;
            _rigidbody.SetVelocityY(currentY => Math.Max(-clampedSinkSpeedBeforeSwim, currentY));

            _rigidbody.velocity += Vector2.up * SwimForce;

            //Restart the swim stroke
            _animator.RestartAnimation();

            //Play sound
            _multiSoundPlayer.PlaySound(index: 1, volume: 0.25f);
        }

        //Clamp speeds
        _rigidbody.SetVelocityY(currentY => Mathf.Clamp(currentY, -MaxSinkSpeed, MaxAscensionSpeed));
        _rigidbody.SetVelocityX(currentX => Mathf.Clamp(currentX, -MaxHorizontalSpeed, MaxHorizontalSpeed));
    }
}
