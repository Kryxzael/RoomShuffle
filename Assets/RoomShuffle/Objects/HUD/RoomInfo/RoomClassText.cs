using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;

/// <summary>
/// Displays the name of the current room's class
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class RoomClassText : MonoBehaviour
{
    private TextMeshProUGUI _label;

    private void Awake()
    {
        _label = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _label.text = Commons.RoomGenerator.CurrentRoomConfig.Class switch
        {
            RoomClass.Inaccessible => "Unknown",
            RoomClass.Starting => "",
            RoomClass.Platforming => "",
            RoomClass.Respite => "",
            RoomClass.Transition => "",
            RoomClass.Shop => "Shop",
            RoomClass.Eradication => "Eradication",
            RoomClass.Puzzle => "Puzzle",
            RoomClass.Secret => "Secret",
            RoomClass.Crossroads => "",
            RoomClass.Boss => "Boss",
            _ => throw new NotImplementedException(),
        };
    }
}