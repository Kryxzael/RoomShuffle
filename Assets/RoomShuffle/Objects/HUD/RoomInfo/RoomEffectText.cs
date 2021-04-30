using System;
using System.Collections;
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
    private String lastText = "";
    private Color _originalColor;

    private TextSmack _textSmack;

    private void Awake()
    {
        _label = GetComponent<TextMeshProUGUI>();
        _textSmack = GetComponent<TextSmack>();

        _originalColor = _label.color;
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
            _ => "Multiple",
        };

        if (!lastText.Equals(_label.text))
        {
            _textSmack.Smack(transform.localScale.x);
            StartCoroutine(CoBlink());
        }

        lastText = _label.text;
    }

    private IEnumerator CoBlink()
    {
        for(int i = 0; i < 7; i++)
        {
            _label.color = Color.clear;
            yield return new WaitForSeconds(0.1f);
            _label.color = _originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}