using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Base class for a script that can detect when something is on it
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class DetectObjectsOn : MonoBehaviour
{
    //Holds the objects that are sticking to the object's surface
    protected HashSet<Transform> ObjectsOn = new HashSet<Transform>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ObjectsOn.Add(collision.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ObjectsOn.Remove(collision.transform);
    }
}