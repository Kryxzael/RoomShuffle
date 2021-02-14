using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Flippable), typeof(Rigidbody2D))]
public class GoombalikeMovementPattern : MonoBehaviour
{
    public float WalkSpeed;

    private Rigidbody2D _rigidbody;
    private Flippable _flippable;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _flippable = GetComponent<Flippable>();
    }

    void Update()
    {
        _rigidbody.SetVelocityX(_flippable.DirectionSign * WalkSpeed);
    }
}
