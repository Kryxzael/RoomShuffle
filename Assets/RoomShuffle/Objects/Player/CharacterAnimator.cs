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
[RequireComponent(typeof(WallJump))]
public class CharacterAnimator : MonoBehaviour
{
    [Header("Animations")]
    [Tooltip("The animation to use when the character isn't moving")]
    public SpriteAnimation Idle;

    [Tooltip("The animation to use when the character is moving on the ground")]
    public SpriteAnimation Walk;

    [Tooltip("The animation to use when the character is moving up")]
    public SpriteAnimation Jump;

    [Tooltip("The animation to use when the character is underwater")]
    public SpriteAnimation Swim;

    [Tooltip("The animation to use when the character is moving down")]
    public SpriteAnimation Fall;

    [Tooltip("The animation to use when the character is sliding down a wall-jumpable wall")]
    public SpriteAnimation Slide;

    [Tooltip("The animation to use when the character is firing a weapon")]
    public SpriteAnimation Fire;

    /* *** */

    private Rigidbody2D _rigid;
    private SpriteAnimator _spriteAnimator;
    private WallJump _wallJump;
    private Flippable _flippable;

    private RenewableLazy<PlayerWeaponShooter> _playerShooter = new RenewableLazy<PlayerWeaponShooter>(() => FindObjectOfType<PlayerWeaponShooter>());

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteAnimator = GetComponent<SpriteAnimator>();
        _flippable = GetComponent<Flippable>();
        _wallJump = GetComponent<WallJump>();
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

        //Shooting
        if ((System.DateTime.Now - Commons.Inventory.LastFireTime).TotalSeconds <= 0.1f)
            _spriteAnimator.Animation = Fire;

        //Swimming
        else if (Water.IsSubmerged(_rigid) && !this.OnGround2D())
            _spriteAnimator.Animation = Swim;

        //Jumping (Moving up)
        else if (_rigid.velocity.y > MIN_MOTION && !this.OnGround2D())
            _spriteAnimator.Animation = Jump;

        //Falling (In the air)
        else if (!this.OnGround2D())
        {
            if (_wallJump.NextToWall)
            {
                _spriteAnimator.Animation = Slide;
            }
            else
            {
                _spriteAnimator.Animation = Fall;
            }
        }


        //Falling (Moving laterally)
        else if (Mathf.Abs(_rigid.velocity.x) > MIN_MOTION)
            _spriteAnimator.Animation = Walk;

        //Idle (Not moving)
        else
            _spriteAnimator.Animation = Idle;
    }
}
