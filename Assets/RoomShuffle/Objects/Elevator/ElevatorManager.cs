using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    [Tooltip("Number of secounds between each elevator")]
    public float Frequency;

    [Tooltip("The elevator object")]
    public GameObject Elevator;

    [Tooltip("Should the blocks return when hitting the endpoint")]
    public bool ShouldReturn;

    [Tooltip("Sets the number of elevators that should spawn. 0 if infinite")]
    public int NumberOfElevators;
    
    [NonSerialized]
    public Vector2 StartPoint;
    
    [NonSerialized]
    public Vector2 EndPoint;

    private int _numberOfElevatorsSpwaned;
    private float _time;
    private void Start()
    {
        StartPoint = transform.Position2D();
        
        foreach (Transform child in transform)
        {
            //Maybe find a better way to get endpoint
            if (child.name.Equals("ElevatorEnd"))
            {
                EndPoint = child.Position2D();
                break;
            }
        }
    }

    void Update()
    {
        if (_numberOfElevatorsSpwaned >= NumberOfElevators && NumberOfElevators != 0)
            return;

        _time += Time.deltaTime;

        if (_time > Frequency)
        {
            _time = 0;

            Instantiate(Elevator, StartPoint, Quaternion.identity, transform);
            _numberOfElevatorsSpwaned++;
        }
    }
}
