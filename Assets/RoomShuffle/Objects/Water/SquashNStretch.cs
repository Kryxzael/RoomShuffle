using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashNStretch : MonoBehaviour
{

    public float Speed;

    public float Magnitude;

    private float _time;
    private Vector2 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    void Update()
    {
        _time += Time.deltaTime * Speed;

        float squash = _originalScale.x + (Mathf.Sin(_time)*Magnitude);
        float squish = _originalScale.y + (Mathf.Cos(_time)*Magnitude);
        

        transform.localScale = new Vector2(squash, squish);
    }
}
