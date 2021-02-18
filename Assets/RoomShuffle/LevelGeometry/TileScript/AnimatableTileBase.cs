using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Base class for tiles that can be animated
/// </summary>
public abstract class AnimatableTileBase : TileBase
{
    [Tooltip("The way the collision on the tile is calculated")]
    public Tile.ColliderType CollisionType = Tile.ColliderType.Grid;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //Sets the collider and sprite of the tile
        tileData.colliderType = CollisionType;
        tileData.sprite = GetStaticSprite();
    }

    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        SpriteAnimation animation = GetAnimation();

        if (animation == null)
            return false;

        //Sets the animation of the tile (if there is one)
        tileAnimationData.animatedSprites = animation.Frames.Select(i => i.Sprite).ToArray();

        tileAnimationData.animationSpeed =
            1f / (animation.FrameTime / animation.FrameTimeDivider); //Converts frame-time to frame-rate since the Unity's animation system wants that

        return true;
    }

    /// <summary>
    /// Gets the animation to display. If this function returns null, the static sprite will be used instead
    /// </summary>
    /// <returns></returns>
    protected abstract SpriteAnimation GetAnimation();

    /// <summary>
    /// Gets the static sprite to use in edit mode and if there is no animation
    /// </summary>
    /// <returns></returns>
    protected abstract Sprite GetStaticSprite();
}
