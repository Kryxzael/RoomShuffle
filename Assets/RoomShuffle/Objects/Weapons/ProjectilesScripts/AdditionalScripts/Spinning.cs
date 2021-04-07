using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float RotationSpeed;

    [Tooltip("How much time must pass before the spinning starts")]
    public float TimeBeforeStart = 0;

    private float _timePassed;
    void Update()
    {
        _timePassed += Time.deltaTime;
        
        if (_timePassed < TimeBeforeStart)
            return;

        transform.Rotate(RotationSpeed * Time.deltaTime);
    }
}
