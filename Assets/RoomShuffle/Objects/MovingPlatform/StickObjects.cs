using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Cases objects to "stick" the object's surface
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class StickObjects : MonoBehaviour
{
    private Vector3 _lastPosition;

    //Holds the objects that are sticking to the object's surface
    private HashSet<Transform> _stuckObjects = new HashSet<Transform>();

    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (transform.position != _lastPosition)
        {
            foreach (Transform i in _stuckObjects)
                i.Translate(transform.position - _lastPosition);

            _lastPosition = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _stuckObjects.Add(collision.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _stuckObjects.Remove(collision.transform);
    }
}
