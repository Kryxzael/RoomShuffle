using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;

/// <summary>
/// Displays the name of the current room's effect
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class RoomEffectText : MonoBehaviour
{
    private TextMeshProUGUI _label;

    private void Awake()
    {
        _label = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _label.text = Commons.RoomGenerator.CurrentRoomConfig.Effect switch
        {
            RoomEffects.None => "",
            RoomEffects.LowGravity => "Low Gravity",
            RoomEffects.Darkness => "Dark",
            RoomEffects.ValuePickups => "2x Pickups",
            RoomEffects.FastFoe => "Fast-Foe",
            RoomEffects.LargeEnemies => "Large Enemies",
            RoomEffects.LargeProjectiles => "Large Projectiles",
            RoomEffects.Backlit => "Back-lit",
            RoomEffects.ReverseControls => "Reversed Controls",
            RoomEffects.Icy => "Slippery",
            RoomEffects.Timer => "Time Limit",
            _ => throw new NotImplementedException(),
        };
    }
}