using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Makes an object move back and forth, turing when it hits a wall
/// </summary>
public class GoombalikeMovementPattern : EnemyScript
{
    public float WalkSpeed;

    [Tooltip("If set, the enemy will move across the Y axis instead of the X axis")]
    public bool Vertical;

    void FixedUpdate()
    {
        float velocity = Enemy.Flippable.DirectionSign * Commons.GetEffectValue(WalkSpeed, EffectValueType.EnemySpeed);

        if (Vertical)
            Enemy.Rigidbody.SetVelocityY(velocity);

        else
            Enemy.Rigidbody.SetVelocityX(velocity);
    }
}
