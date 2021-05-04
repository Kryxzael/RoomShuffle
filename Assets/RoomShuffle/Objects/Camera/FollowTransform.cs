using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Makes an object smoothly follow a transform on all axises except Z
/// </summary>
public class FollowTransform : MonoBehaviour
{
    [Tooltip("The object to follow")]
    public Transform Target;

    private void Update()
    {
        if (!Target)
            return;

        //Update the position
        transform.position = Target.transform.position.SetZ(transform.position.z);
    }
}
