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

    [Tooltip("The 'arms' that points to the pivot point")]
    public GameObject Arm;

    private float _time;
    private Vector3 _pivotPoint;
    private GameObject _armTransform;
    void Start()
    {
        _pivotPoint = transform.position;

        if (Arm)
        {
            GameObject instance = Instantiate(Arm, transform.position, Quaternion.identity);
            _armTransform = instance;
        }



        if (_armTransform)
        {
           _armTransform.GetComponent<SpriteRenderer>().size = new Vector2(1, Radius);
        }
    }
    
    void Update()
    {
        //Sets the position of the object relative to its center (pivot point)
        _time += Time.deltaTime * Speed;
        float x = Mathf.Cos(_time) * Radius;
        float y = Mathf.Sin(_time) * Radius;

        transform.localPosition =  new Vector3(x, y, 0);

        float RotationDistance = (float) Math.PI * 2;

        //Keeps the _time variable low
        if (_time > RotationDistance)
        {
            _time -= RotationDistance;
        }

        //Rotate Arm
        if (_armTransform)
        {
            float parentRotation = transform.parent.GetEuler().z;
            
            _armTransform.transform.rotation = Quaternion.Euler(new Vector3(0,0, _time * (360 / RotationDistance) - 90 + parentRotation));
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
