using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EndOfLineOption
{
    Destroy,
    Loop,
    Return
}

public class ElevatorManager : MonoBehaviour
{
    [Tooltip("Number of secounds between each elevator")]
    public float Frequency;

    [Tooltip("The elevator object")]
    public Elevator ElevatorObject;

    [Tooltip("Sets the number of elevators that should spawn. 0 if infinite")]
    public int NumberOfElevators;
    
    [Tooltip("What the elevator should do at the end of the line")]
    public EndOfLineOption EOLOption;

    [NonSerialized]
    public Vector2 StartPoint;
    
    [NonSerialized]
    public Vector2 EndPoint;

    private int _numberOfElevatorsSpwaned;
    private float _time;
    private List<Vector2> _checkpointList = new List<Vector2>();
    private bool _closeLoop = false;
    private float _totalLoopDistance;
    private void Start()
    {
        StartPoint = transform.Position2D();
        
        _checkpointList.Add(StartPoint);
        
        foreach (Transform child in transform)
        {
            //Maybe find a better way to get endpoint
            if (child.name.Equals("ElevatorEnd"))
            {
                EndPoint = child.Position2D();
            } 
            else if (child.name.Contains("Checkpoint"))
            {
                _checkpointList.Add(child.Position2D());
            }
        }
        
        _checkpointList.Add(EndPoint);

        //adds the closing loop distance 
        _totalLoopDistance += Vector2.Distance(_checkpointList.First(), _checkpointList.Last());

        //Add space between checkpoints to total length.
        for (int i = 0; i < _checkpointList.Count-1; i++)
        {
            _totalLoopDistance += Vector2.Distance(_checkpointList[i], _checkpointList[i + 1]);
        }

    }

    void Update()
    {
        if ((_numberOfElevatorsSpwaned >= NumberOfElevators && NumberOfElevators != 0) || _closeLoop)
            return;


        _time += Time.deltaTime;
        
        //Dynamic spacing between 
        if (EOLOption == EndOfLineOption.Loop && NumberOfElevators != 0)
        {
            float speed = ElevatorObject.Speed;

            if (_totalLoopDistance / speed / NumberOfElevators * _numberOfElevatorsSpwaned < _time)
            {
                Instantiate(ElevatorObject, StartPoint, Quaternion.identity, transform);
                _numberOfElevatorsSpwaned++;
                return;
            }
        } 

        if (_time > Frequency)
        {
            _time = 0;

            Instantiate(ElevatorObject, StartPoint, Quaternion.identity, transform);
            _numberOfElevatorsSpwaned++;
        }
    }

    public List<Vector2> GetCheckpointList()
    {
        return _checkpointList;
    }

    /// <summary>
    /// Stops the manager from making more objects.
    /// </summary>
    public void CloseLoop()
    {
        _closeLoop = true;
    }
}
