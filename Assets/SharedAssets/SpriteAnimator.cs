using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Allows a sprite to be animated using a SpriteAnimation
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    /// <summary>
    /// This field is used to check every frame if the current animation has changed.
    /// </summary>
    private SpriteAnimation _animation;

    /// <summary>
    /// Gets or sets the current animation. Setting an animation will reset the CurrentFrame value
    /// </summary>
    public SpriteAnimation Animation;

    /// <summary>
    /// The speed the animation will be played back at (percentage)
    /// </summary>
    public float PlaybackSpeed = 1f;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_animation == Animation)
        {
            return;
        }

        _animation = Animation;

        StopAllCoroutines();
        if (_animation == null || _animation.FrameCount == 0)
        {
            return;
        }

        StartCoroutine(nameof(Animate));
    }

    private int _currentFrame;

    /// <summary>
    /// Gets or set the current frame of animation
    /// </summary>
    public int CurrentFrame
    {
        get
        {
            return _currentFrame;
        }
        set
        {
            _currentFrame = Mathf.Clamp(value, -1, _animation.FrameCount);
        }
    }

    /// <summary>
    /// Restarts the current animation
    /// </summary>
    public void RestartAnimation()
    {
        if (_animation == null)
            return;

        StopAllCoroutines();
        CurrentFrame = -1;
        StartCoroutine(nameof(Animate));
    }

    private IEnumerator Animate()
    {
        CurrentFrame = -1;

        if (_animation.FrameCount == 0)
        {
            yield break;
        }

        while (true)
        {
            CurrentFrame = (CurrentFrame + 1) % _animation.FrameCount;
            _spriteRenderer.sprite = _animation.Frames[CurrentFrame].Sprite;

            yield return new WaitForSeconds(_animation.FrameTime / _animation.FrameTimeDivider * _animation.Frames[CurrentFrame].FrameTimeMultiplier / PlaybackSpeed);
        }
    }

    /// <summary>
    /// Restarts the animation if the object have previously been disabled and enabled again
    /// </summary>
    private void OnEnable()
    {
        RestartAnimation();
    }
}
