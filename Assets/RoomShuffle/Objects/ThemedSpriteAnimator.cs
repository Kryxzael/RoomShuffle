using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(SpriteAnimator))]
public class ThemedSpriteAnimator : MonoBehaviour
{
    private SpriteAnimator _animator;

    [Tooltip("The set of animations to use")]
    public ThemedAnimationCollection Animations;

    private void Awake()
    {
        _animator = GetComponent<SpriteAnimator>();
    }

    private void Update()
    {
        _animator.Animation = Animations.GetCurrentThemeAnimation();
    }
}
