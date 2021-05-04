using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An animatable tile that can change its sprite/animation based on a room generator's settings
/// </summary>
[CreateAssetMenu(menuName = "2D/Themed Tile")]
public class ThemedTile : AnimatableTileBase
{
    /// <summary>
    /// Gets the sprites and animations to use
    /// </summary>
    public ThemedAnimationCollection Visuals;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    protected override SpriteAnimation GetAnimation()
    {
        return Visuals.GetCurrentThemeAnimation();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    protected override Sprite GetStaticSprite()
    {
        return Visuals.GetCurrentThemeSprite();
    }
}
