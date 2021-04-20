using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextSmack), typeof(TextMeshProUGUI))]
public class RoomCounter : MonoBehaviour
{
    private int _lastRoomNumber;
    private TextMeshProUGUI _textMeshProUGUI;
    private TextSmack _textSmack;

    private void Start()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        _textSmack = GetComponent<TextSmack>();
    }

    private void Update()
    {
        if (_lastRoomNumber != (_lastRoomNumber = Commons.RoomGenerator.CurrentRoomNumber))
        {
            _textMeshProUGUI.text = _lastRoomNumber.ToString();
            _textSmack.Smack();
        }
        
    }
}
