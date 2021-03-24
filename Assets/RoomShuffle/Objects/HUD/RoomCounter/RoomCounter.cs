using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextSmack), typeof(TextMeshProUGUI))]
public class RoomCounter : MonoBehaviour
{
    
    private int roomNumber = 0;
    private int x;
    private TextMeshProUGUI _textMeshProUGUI;
    private TextSmack _textSmack;

    private void Start()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        _textSmack = GetComponent<TextSmack>();
    }

    private void Update()
    {
        //TODO remove all the nonsense below
        if (Input.GetKeyDown(KeyCode.O))
        {
            x++;
        }
        if (roomNumber == x)
            return;
        roomNumber = x;
        //Nonsense ends here
        
        
        _textMeshProUGUI.text = roomNumber.ToString();
        StartCoroutine(_textSmack.Smack());
        
    }
}
