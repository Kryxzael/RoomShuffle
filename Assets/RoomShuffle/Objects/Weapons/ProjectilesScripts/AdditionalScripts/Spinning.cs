using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float RotationSpeed;

    [Tooltip("How much time must pass before the spinning starts")]
    public float TimeBeforeStart = 0;

    public bool AffectedByFastFoe = true;

    private float _timePassed;
    void Update()
    {
        _timePassed += Time.deltaTime;
        
        if (_timePassed < TimeBeforeStart)
            return;

        var speed = RotationSpeed;

        if (AffectedByFastFoe)
            speed = Commons.GetEffectValue(speed, EffectValueType.EnemySpeed);

        transform.Rotate(speed * Time.deltaTime);
    }
}
