using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Cases objects to "stick" the object's surface
/// </summary>

public class StickObjects : DetectObjectsOn
{
    private Vector3 _lastPosition;

    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (transform.position != _lastPosition)
        {
            ObjectsOn.RemoveWhere(i => i == null);

            foreach (Transform i in ObjectsOn)
                i.Translate(transform.position - _lastPosition);

            _lastPosition = transform.position;
        }
    }

    
}
