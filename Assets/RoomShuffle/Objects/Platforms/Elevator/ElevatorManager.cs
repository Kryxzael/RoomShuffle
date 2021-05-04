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

/// <summary>
/// Manages an elevator track
/// </summary>
public class ElevatorManager : MonoBehaviour
{
    private const int INFINITE_ELEVATORS = 0;

    [Tooltip("Number of seconds between each elevator cart")]
    public float Frequency;

    [Tooltip("Sets the number of elevator carts that should spawn. 0 or less = infinite")]
    public int NumberOfCarts;

    [Header("Cart Settings")]
    [Tooltip("The elevator cart prefab")]
    public Elevator CartPrefab;

    [Tooltip("What the elevator cart should do at the end of the line")]
    public EndOfLineOption EOLOption;

    [Tooltip("How quickly the carts should move")]
    public float CartSpeed = 5;

    [Tooltip("The width of the elevator carts")]
    public int CartWidth = 4;

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
        int numberOfElevators = NumberOfCarts;

        if (numberOfElevators <= INFINITE_ELEVATORS)
            numberOfElevators = 9999999;

        //Elevator set to loop mode. Carts should be spawned with equal spacing
        if (EOLOption == EndOfLineOption.Loop)
        {
            if (numberOfElevators == 9999999)
            {
                Debug.LogWarning("Looping elevator cannot have infinite carts");
                yield break;
            }

            for (int i = 0; i < numberOfElevators; i++)
            {
                Instantiate(CartPrefab, StartPoint.position, Quaternion.identity, transform);
                yield return new WaitForSeconds(TrackLength / CartSpeed / NumberOfCarts);
            }
        }

        //Elevator does not dynamically space carts
        else
        {
            for (int i = 0; i < numberOfElevators; i++)
            {
                Instantiate(CartPrefab, StartPoint.position, Quaternion.identity, transform);
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
