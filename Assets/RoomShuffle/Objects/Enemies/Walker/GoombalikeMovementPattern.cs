using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GoombalikeMovementPattern : EnemyScript
{
    public float WalkSpeed;

    void FixedUpdate()
    {
        Enemy.Rigidbody.SetVelocityX(Enemy.Flippable.DirectionSign * Commons.GetEffectValue(WalkSpeed, EffectValueType.EnemySpeed));
    }
}
