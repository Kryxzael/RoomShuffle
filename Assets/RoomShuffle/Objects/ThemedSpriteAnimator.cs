using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(SpriteAnimator))]
[RequireComponent(typeof(SpriteRenderer))]
public class ThemedSpriteAnimator : MonoBehaviour
{
    private SpriteAnimator _animator;
    private SpriteRenderer _renderer;

    [Tooltip("The set of animations to use")]
    public ThemedAnimationCollection Animations;

    private void Awake()
    {
        _animator = GetComponent<SpriteAnimator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var animation = Animations.GetCurrentThemeAnimation();

        if (animation == null)
        {
            _renderer.sprite = Animations.GetCurrentThemeSprite();
        }
        else
        {
            _animator.Animation = Animations.GetCurrentThemeAnimation();
        }
    }
}
