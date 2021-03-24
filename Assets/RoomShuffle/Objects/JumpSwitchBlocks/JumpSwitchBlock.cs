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
public class JumpSwitchBlock : MonoBehaviour
{
    private Collider2D _collider;
    private SpriteAnimator _animator;

    /* *** */

    [Tooltip("Is the switch block initially solid when it starts")]
    public bool InitiallyOn;

    [Header("Appearance")]
    [Tooltip("The animation to use when the block is solid")]
    public SpriteAnimation OnAnimation;

    [Tooltip("The animation to use when the block is non-solid")]
    public SpriteAnimation OffAnimation;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<SpriteAnimator>();

        _collider.enabled = InitiallyOn;
        SetSprite();
    }

    /// <summary>
    /// Switches the solidity of the block
    /// </summary>
    public void Switch()
    {
        _collider.enabled = !_collider.enabled;
        SetSprite();
    }

    /// <summary>
    /// Updates the sprite of the jump switch block
    /// </summary>
    private void SetSprite()
    {
        if (_collider.enabled)
        {
            _animator.Animation = OnAnimation;
        }
        else
        {
            _animator.Animation = OffAnimation;
        }
    }
}
