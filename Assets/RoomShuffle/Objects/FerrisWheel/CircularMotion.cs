using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    [Tooltip("The speed of the object")] 
    public float Speed;

    [Tooltip("The radius of the circle")] 
    public float Radius;

    private float _time;
    private Vector3 _pivotPoint;
    void Start()
    {
        _pivotPoint = transform.position;
    }
    
    void Update()
    {
        //Sets the position of the object relative to its center (pivot point)
        _time += Time.deltaTime * Speed;
        float x = Mathf.Cos(_time) * Radius;
        float y = Mathf.Sin(_time) * Radius;

        transform.localPosition =  new Vector3(x, y, 0);

        //Keeps the _time variable low
        if (_time > (float)Math.PI * 2)
        {
            _time -= (float)Math.PI * 2;
        }
    }

    /// <summary>
    /// Sets the position relative to its center. Takes an argument between 0 and 360
    /// </summary>
    /// <param name="angle"></param>
    public void SetAngle(float angle)
    {
        _time = (float) Math.PI * 2 / 360 * angle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_pivotPoint, Radius);
    }
}
