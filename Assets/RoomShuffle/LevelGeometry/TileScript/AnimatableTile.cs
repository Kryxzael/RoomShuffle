using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Main implementation of a tile that can use a SpriteAnimation as its texture
/// </summary>
[CreateAssetMenu(menuName = "2D/Animatible Tile")]
public class AnimatableTile : AnimatableTileBase
{
    [Header("Appearance")]
    [Tooltip("The sprite to display on the tile when there is no animation, or in edit mode")]
    public Sprite NonAnimatedSprite;

    [Tooltip("The (optional) animation to render on the tile")]
    public SpriteAnimation Animation;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    protected override Sprite GetStaticSprite()
    {
        return NonAnimatedSprite;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    protected override SpriteAnimation GetAnimation()
    {
        return Animation;
    }
}