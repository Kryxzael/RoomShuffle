using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A block that will switch its solidity any time the player jumps
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteAnimator))]
public class JumpSwitchBlock : JumpSwitchBase
{
    protected Collider2D Collider;
    private SpriteAnimator _animator;

    /* *** */

    [Tooltip("Is the switch block initially solid when it starts")]
    public bool InitiallyOn;

    [Header("Appearance")]
    [Tooltip("The animation to use when the block is solid")]
    public SpriteAnimation OnAnimation;

    [Tooltip("The animation to use when the block is non-solid")]
    public SpriteAnimation OffAnimation;

    protected virtual void Start()
    {
        Collider = GetComponent<Collider2D>();
        _animator = GetComponent<SpriteAnimator>();

        Collider.enabled = InitiallyOn;
        SetSprite();
    }

    /// <summary>
    /// Switches the solidity of the block
    /// </summary>
    public override void OnJump()
    {
        Collider.enabled = !Collider.enabled;
        SetSprite();
    }

    /// <summary>
    /// Updates the sprite of the jump switch block
    /// </summary>
    protected void SetSprite()
    {
        if (Collider.enabled)
        {
            _animator.Animation = OnAnimation;
        }
        else
        {
            _animator.Animation = OffAnimation;
        }
    }
}
