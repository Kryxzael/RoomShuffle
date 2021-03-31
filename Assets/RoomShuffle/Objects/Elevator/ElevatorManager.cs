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
    private static int INFINITE_ELEVATORS = 0;

    [Tooltip("Number of seconds between each elevator")]
    public float Frequency;

    [Tooltip("The elevator object")]
    public Elevator ElevatorObject;

    [Tooltip("Sets the number of elevators that should spawn. 0 or less = infinite")]
    public int NumberOfElevators;
    
    [Tooltip("What the elevator should do at the end of the line")]
    public EndOfLineOption EOLOption;

    /// <summary>
    /// Gets all the checkpoints of the elevator
    /// </summary>
    public IEnumerable<Transform> Checkpoints
    {
        get
        {
            //Gets all children (except elevators)
            foreach (Transform i in transform)
            {
                if (i.GetComponent<Elevator>())
                    continue;

                yield return i;
            }   
        }
    }

    /// <summary>
    /// Gets the transform that acts as the starting point for platforms
    /// </summary>
    public Transform StartPoint
    {
        get
        {
            return Checkpoints.First();
        }
    }

    /// <summary>
    /// Gets the transform that acts as the ending point for platforms
    /// </summary>
    public Transform EndPoint
    {
        get
        {
            return Checkpoints.Last();
        }
    }

    /// <summary>
    /// Gets the total length of the elevator
    /// </summary>
    public float TrackLength
    {
        get
        {
            Transform last = StartPoint;
            float distance = 0f;

            foreach (Transform i in Checkpoints.Skip(1))
            {
                distance += Vector2.Distance(i.position, last.position);
                last = i;
            }

            if (EOLOption == EndOfLineOption.Loop)
                distance += Vector2.Distance(StartPoint.position, last.position);

            return distance;
        }
    }

    private IEnumerator Start()
    {
        //Sets the amount of elevators to spawn
        float numberOfElevators = NumberOfElevators;

        if (numberOfElevators <= INFINITE_ELEVATORS)
            numberOfElevators = float.PositiveInfinity;

        //Elevator set to loop mode. Carts should be spawned with equal spacing
        if (EOLOption == EndOfLineOption.Loop)
        {
            if (float.IsInfinity(numberOfElevators))
            {
                Debug.LogWarning("Looping elevator cannot have infinite carts");
                yield break;
            }

            for (int i = 0; i < numberOfElevators; i++)
            {
                Instantiate(ElevatorObject, StartPoint.position, Quaternion.identity, transform);
                yield return new WaitForSeconds(TrackLength / ElevatorObject.Speed / NumberOfElevators);
            }
        }

        //Elevator does not dynamically space carts
        else
        {
            for (int i = 0; i < numberOfElevators; i++)
            {
                Instantiate(ElevatorObject, StartPoint.position, Quaternion.identity, transform);
                yield return new WaitForSeconds(Frequency);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var lastPoint = StartPoint;

        foreach (var i in Checkpoints)
        {
            Gizmos.DrawLine(lastPoint.position, i.position);
            lastPoint = i;
        }

        if (EOLOption == EndOfLineOption.Loop)
            Gizmos.DrawLine(EndPoint.position, StartPoint.position);
    }
}
