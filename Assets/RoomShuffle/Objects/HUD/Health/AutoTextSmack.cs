using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoTextSmack : TextSmack
{
    private TextMeshProUGUI _textMeshProUGUI;
    private TextMeshPro _textMeshPro;
    private string _lastText;
    void Start()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        if (!_textMeshPro)
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }
    }
    
    void Update()
    {
        if (_textMeshPro)
        {
            if (_textMeshPro.text != _lastText)
            {
                _lastText = _textMeshPro.text;
                Smack();
            }
        } 
        else if (_textMeshProUGUI)
        {
            if (_textMeshProUGUI.text != _lastText)
            {
                _lastText = _textMeshProUGUI.text;
                Smack();
            }
        }
    }
}
