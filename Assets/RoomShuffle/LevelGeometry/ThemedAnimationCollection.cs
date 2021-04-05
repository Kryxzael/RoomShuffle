using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Holds sprites and animations for different themes
/// </summary>
[Serializable]
public class ThemedAnimationCollection
{
    //Holds the last fetched room generator instance.
    private static RoomGenerator _bufferedRoomGenerator;

    /// <summary>
    /// Gets the current room generator, or null if there is one
    /// </summary>
    private static RoomGenerator RoomGenerator
    {
        get
        {
            //There is no buffered room generator, or it has been destroyed, try to fetch a new one
            if (_bufferedRoomGenerator == null)
                _bufferedRoomGenerator = UnityEngine.Object.FindObjectOfType<RoomGenerator>();

            return _bufferedRoomGenerator;
        }
    }

    /* *** */

    [Header("Sprites")]
    [Tooltip("The fallback sprite that is used when no recognized theme is found")]
    public Sprite DefaultSprite;

    public Sprite GrassSprite;
    public Sprite SnowSprite;
    public Sprite AutumnSprite;
    public Sprite CaveSprite;
    public Sprite VolcanoSprite;
    public Sprite JungleSprite;
    public Sprite CloudSprite;
    public Sprite FactorySprite;

    [Header("Animations")]
    [Tooltip("The fallback animation that is used when no recognized theme is found")]
    public SpriteAnimation DefaultAnimation;

    public SpriteAnimation GrassAnimation;
    public SpriteAnimation SnowAnimation;
    public SpriteAnimation AutumnAnimation;
    public SpriteAnimation CaveAnimation;
    public SpriteAnimation VolcanoAnimation;
    public SpriteAnimation JungleAnimation;
    public SpriteAnimation CloudAnimation;
    public SpriteAnimation FactoryAnimation;

    /// <summary>
    /// Gets the sprite for the current tileset according to the room generator
    /// </summary>
    /// <returns></returns>
    public Sprite GetCurrentThemeSprite()
    {
        return GetCurrentThemeResources().sprite;
    }

    /// <summary>
    /// Gets the animation for the current tileset according to the room generator
    /// </summary>
    /// <returns></returns>
    public SpriteAnimation GetCurrentThemeAnimation()
    {
        return GetCurrentThemeResources().animation;
    }

    /// <summary>
    /// Gets the sprite and animation for the current tileset according to the room generator
    /// </summary>
    /// <returns></returns>
    private (Sprite sprite, SpriteAnimation animation) GetCurrentThemeResources()
    {
        if (RoomGenerator != null && RoomGenerator.History.Any())
        {
            return RoomGenerator.CurrentRoomConfig.Theme switch
            {
                RoomTheme.Grass => (GrassSprite, GrassAnimation),
                RoomTheme.Snow => (SnowSprite, SnowAnimation),
                RoomTheme.Autumn => (AutumnSprite, AutumnAnimation),
                RoomTheme.Cave => (CaveSprite, CaveAnimation),
                RoomTheme.Volcano => (VolcanoSprite, VolcanoAnimation),
                RoomTheme.Jungle => (JungleSprite, JungleAnimation),
                RoomTheme.Cloud => (CloudSprite, CloudAnimation),
                RoomTheme.Factory => (FactorySprite, FactoryAnimation),
                _ => (DefaultSprite, DefaultAnimation)
            };
        }

        return (DefaultSprite, DefaultAnimation);
    }
}
