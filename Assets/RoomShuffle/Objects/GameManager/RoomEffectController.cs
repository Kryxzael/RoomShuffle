using System;
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

    [Header("Icy")]
    [Tooltip("By how much the player's acceleration speed will be multiplied when icy is enabled")]
    public float IcyGroundAccelerationMultiplier = 1f;

    [Tooltip("By how much the player's deceleration speed will be multiplied when icy is enabled")]
    public float IcyGroundDecelerationMultiplier = 1f;

    [Tooltip("By how much the player's maximum ground speed will be multiplied when icy is enabled")]
    public float IcyGroundMaxSpeedMultiplier = 1.2f;

    /* *** */

    //The default gravity level as defined by the physics settings
    private Vector2 _defaultGravity = new Vector2(0, -60);

    /// <summary>
    /// Runs when a new room is generated
    /// </summary>
    public void OnRoomStart(RoomParameters room)
    {
        RoomEffects fx = room.Effect;

        LowGravity(fx.HasFlag(RoomEffects.LowGravity));
        Darkness(fx.HasFlag(RoomEffects.Darkness));
        LargeEnemies(fx.HasFlag(RoomEffects.LargeEnemies));
        Timer(fx.HasFlag(RoomEffects.Timer), room);
    }

    private void Update()
    {
        if ((Commons.CurrentRoomEffects.HasFlag(RoomEffects.Timer) || Commons.SpeedRunMode) && Commons.CountdownTimer.CurrentSeconds <= 0)
        {
            if (Commons.SpeedRunMode)
            {
                Commons.PlayerHealth.Kill();
            }
            else
            {
                Commons.PlayerHealth.SoftKill(); 
                Commons.RespawnPlayer();
            }

            Commons.CountdownTimer.StopCountdown();

            if (Commons.PlayerHealth.IsAlive)
                Timer(true, Commons.RoomGenerator.CurrentRoomConfig);
        }
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
        foreach (Light i in FindObjectsOfType<Light>().Where(i => i.type == LightType.Directional))
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

                //This assumes that the object is pivoted at its center
                if (enabled)
                {
                    i.transform.TranslateY(height / 2);
                    i.transform.localScale *= LargeEnemiesGrowMultiplier;
                }

                //Disabling this effect does nothing
            }
        }
    }

    /// <summary>
    /// Sets the timer effect
    /// </summary>
    private void Timer(bool enabled, RoomParameters room)
    {
        if (Commons.SpeedRunMode)
            return;
        
        if (enabled)
        {
            Commons.CountdownTimer.ResetCountdown(room.Layout.TimerEffectSeconds * 2f);
        }
        else
        {
            Commons.CountdownTimer.StopCountdown();
            Commons.CountdownTimer.HideTimer();
        }
    }
}