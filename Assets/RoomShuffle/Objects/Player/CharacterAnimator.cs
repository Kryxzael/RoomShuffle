using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UnityEngine;

/// <summary>
/// Applies conditional animations to characters based on their state
/// </summary>
[RequireComponent(typeof(SpriteAnimator))]
[RequireComponent(typeof(Flippable))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterAnimator : MonoBehaviour
{
    [Header("Animations")]
    [Tooltip("The animation to use when the character isn't moving")]
    public SpriteAnimation Idle;

    [Tooltip("The animation to use when the character is moving on the ground")]
    public SpriteAnimation Walk;

    [Tooltip("The animation to use when the character is moving up")]
    public SpriteAnimation Jump;

    [Tooltip("The animation to use when the character is moving down")]
    public SpriteAnimation Fall;

    /* *** */

    private Rigidbody2D _rigid;
    private SpriteAnimator _spriteAnimator;
    private Flippable _flippable;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteAnimator = GetComponent<SpriteAnimator>();
        _flippable = GetComponent<Flippable>();
    }

    void Update()
    {
        const float MIN_MOTION = 0.5f;

        /*
         * Override animation is noclip is enabled
         * 
         */
        if (Cheats.Noclip)
        {
            _flippable.Direction = Direction1D.Right;
            _spriteAnimator.Animation = Idle;
            return;
        }

        /*
         * Set sprite direction
         */
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (FlipCamera.IsFlipped ^ Commons.CurrentRoomEffects.HasFlag(RoomEffects.ReverseControls))
            horizontal *= -1;

        if (horizontal > 0.2f)
            _flippable.Direction = Direction1D.Right;

        else if (horizontal < -0.2f)
            _flippable.Direction = Direction1D.Left;

        /*
         * Set animation
         */

        //Jumping (Moving up)
        else if (_rigid.velocity.y > MIN_MOTION && !this.OnGround2D())
            _spriteAnimator.Animation = Jump;

        //Falling (In the air)
        else if (!this.OnGround2D())
            _spriteAnimator.Animation = Fall;

        //Falling (Moving laterally)
        else if (Mathf.Abs(_rigid.velocity.x) > MIN_MOTION)
            _spriteAnimator.Animation = Walk;

        //Idle (Not moving)
        else
            _spriteAnimator.Animation = Idle;
    }
}
