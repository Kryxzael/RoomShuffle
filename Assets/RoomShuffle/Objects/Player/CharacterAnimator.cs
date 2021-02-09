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

    [Tooltip("The animation to use when the character crouching")]
    public SpriteAnimation Crouch;

    [Tooltip("The animation to use when the character is looking up")]
    public SpriteAnimation LookUp;

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
         * Set sprite direction
         */
        if (_rigid.velocity.x > MIN_MOTION)
            _flippable.Direction = Direction1D.RIGHT;

        else if (_rigid.velocity.x < -MIN_MOTION)
            _flippable.Direction = Direction1D.LEFT;

        /*
         * Set animation
         */

        //Crouching
        if (Input.GetAxisRaw("Vertical") > 0.25f)
            _spriteAnimator.Animation = Crouch;

        //Jumping (Moving up)
        else if (_rigid.velocity.y > MIN_MOTION && !this.OnGround2D())
            _spriteAnimator.Animation = Jump;

        //Falling (In the air)
        else if (!this.OnGround2D())
            _spriteAnimator.Animation = Fall;

        //Falling (Moving laterally)
        else if (Mathf.Abs(_rigid.velocity.x) > MIN_MOTION)
            _spriteAnimator.Animation = Walk;

        //Looking up
        else if (Input.GetAxisRaw("Vertical") < -0.25f)
            _spriteAnimator.Animation = LookUp;

        //Idle (Not moving)
        else
            _spriteAnimator.Animation = Idle;
    }
}
