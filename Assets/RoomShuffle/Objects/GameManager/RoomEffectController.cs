using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Handles most of the room effect 
/// </summary>
public class RoomEffectController : MonoBehaviour
{

    [Header("Low Gravity")]
    [Tooltip("The percentage of normal gravity that will be applied when low gravity is enabled")]
    public float LowGravityMultiplier = 0.5f;

    //The default gravity level as defined by the physics settings
    private Vector2 DefaultGravity;

    /// <summary>
    /// Gets the current room effects
    /// </summary>
    public RoomEffects CurrentEffects
    {
        get
        {
            return Commons.RoomGenerator.CurrentRoomConfig?.Effect ?? RoomEffects.None;
        }
    }

    private void Awake()
    {
        DefaultGravity = Physics2D.gravity;
    }

    /// <summary>
    /// Runs when a new room is generated
    /// </summary>
    public void OnRoomStart(RoomParameters room)
    {
        RoomEffects fx = room.Effect;

        LowGravity(fx.HasFlag(RoomEffects.LowGravity));
    }

    /// <summary>
    /// Sets the low gravity effect
    /// </summary>
    /// <param name="enabled"></param>
    private void LowGravity(bool enabled)
    {
        if (enabled)
            Physics2D.gravity = DefaultGravity * LowGravityMultiplier;

        else
            Physics2D.gravity = DefaultGravity;
    }
}