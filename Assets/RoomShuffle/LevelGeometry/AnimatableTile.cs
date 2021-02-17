using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Animatible Tile")]
public class AnimatableTile : TileBase
{
    [Header("Required")]
    [Tooltip("The sprite to display on the tile when there is no animation, or in edit mode")]
    public Sprite NonAnimatedSprite;

    [Tooltip("The way the collision on the tile is calculated")]
    public Tile.ColliderType CollisionType = Tile.ColliderType.Grid;

    [Header("Optional")]
    [Tooltip("The (optional) animation to render on the tile")]
    public SpriteAnimation Animation;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //Sets the collider and sprite of the tile
        tileData.colliderType = CollisionType;
        tileData.sprite = NonAnimatedSprite;
    }

    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        if (Animation == null)
            return false;

        //Sets the animation of the tile (if there is one)
        tileAnimationData.animatedSprites = Animation.Frames.Select(i => i.Sprite).ToArray();

        tileAnimationData.animationSpeed =
            1f / (Animation.FrameTime / Animation.FrameTimeDivider); //Converts frame-time to frame-rate since the Unity's animation system wants that

        return true;
    }
}