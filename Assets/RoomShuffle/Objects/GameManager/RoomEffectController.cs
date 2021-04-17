﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Handles most of the room effect 
/// </summary>
public class RoomEffectController : MonoBehaviour
{

    [Header("Low Gravity")]
    [Tooltip("The percentage of normal gravity that will be applied when low gravity is enabled")]
    public float LowGravityMultiplier = 0.5f;

    [Header("Value Pickups")]
    [Tooltip("By how much currency will be multiples (rounded to the closes int) when value pickups is enabled")]
    public float ValuePickupsMultiplier = 2f;

    [Header("Fast Foe")]
    [Tooltip("How much faster enemies will move when fast-foe is enabled")]
    public float FastFoeSpeedMultiplier = 1.5f;

    [Header("Large Enemies")]
    [Tooltip("By how much enemies will grow when large enemies are enabled")]
    public float LargeEnemiesGrowMultiplier = 2f;

    [Header("Large Projectiles")]
    [Tooltip("By how much enemies will grow when large enemies are enabled")]
    public float LargeProjectilesGrowMultiplier = 3f;

    /* *** */

    //The default gravity level as defined by the physics settings
    private Vector2 _defaultGravity;

    //The 'sun' sources in the generator that are disabled when dark mode is enabled
    private IEnumerable<Light> _suns;

    private void Awake()
    {
        _defaultGravity = Physics2D.gravity;
        _suns = FindObjectsOfType<Light>().Where(i => i.type == LightType.Directional);
    }

    /// <summary>
    /// Runs when a new room is generated
    /// </summary>
    public void OnRoomStart(RoomParameters room)
    {
        RoomEffects fx = room.Effect;

        LowGravity(fx.HasFlag(RoomEffects.LowGravity));
        Darkness(fx.HasFlag(RoomEffects.Darkness));
        LargeEnemies(fx.HasFlag(RoomEffects.LargeEnemies));
        Backlit(fx.HasFlag(RoomEffects.Backlit));
    }

    /// <summary>
    /// Sets the low gravity effect
    /// </summary>
    /// <param name="enabled"></param>
    private void LowGravity(bool enabled)
    {
        if (enabled)
            Physics2D.gravity = _defaultGravity * LowGravityMultiplier;

        else
            Physics2D.gravity = _defaultGravity;
    }

    /// <summary>
    /// Sets the darkness effect
    /// </summary>
    /// <param name="enabled"></param>
    private void Darkness(bool enabled)
    {
        foreach (Light i in _suns)
            i.enabled = !enabled;
    }

    /// <summary>
    /// Sets the larger enemies effect
    /// </summary>
    /// <param name="enabled"></param>
    private void LargeEnemies(bool enabled)
    {
        foreach (var i in FindObjectsOfType<EnemyBase>())
        {
            if (i.GetComponentInChildren<Collider2D>() is Collider2D collider)
            {
                float height = collider.bounds.size.y;

                //TODO: This assumes that the object is pivoted at its center
                if (enabled)
                {
                    i.transform.TranslateY(height / 2);
                    i.transform.localScale *= LargeEnemiesGrowMultiplier;
                }

                //TODO: Disabling this effect does nothing
            }
        }
    }

    /// <summary>
    /// Sets the back-lit effect
    /// </summary>
    /// <param name="enabled"></param>
    private void Backlit(bool enabled)
    {
        //TODO: This should be removed when we get backgrounds
        if (Camera.main)
            Camera.main.backgroundColor = enabled ? Color.white : Color.black;

        foreach (Light i in FindObjectsOfType<Light>(includeInactive: false))
        {
            i.enabled = !enabled;
        }
    }
}