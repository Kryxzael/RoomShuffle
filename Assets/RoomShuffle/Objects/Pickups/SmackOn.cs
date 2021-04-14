using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextSmack))]
public class SmackOn : MonoBehaviour
{
    public bool SmackOnStart;
    public bool SmackOnEnable;

    private TextSmack _textSmack;
    private void Awake()
    {
        _textSmack = GetComponent<TextSmack>();
    }
    private void Start()
    {
        if (SmackOnStart)
            _textSmack.Smack();
    }
    private void OnEnable()
    {
        if (SmackOnEnable)
            _textSmack.Smack();
    }
}
