using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float RotationSpeed;
    void Update()
    {
        transform.Rotate(RotationSpeed * Time.deltaTime);
    }
}
