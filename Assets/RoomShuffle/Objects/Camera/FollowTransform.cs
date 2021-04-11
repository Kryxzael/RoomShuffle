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

    [Tooltip("The maximum distance between the camera and the target before the camera starts to follow the target")]
    public Vector2 Deadzone;

    [Tooltip("How smooth the camera motion will be. 0 is no smoothening")]
    [Range(0f, 1f)]
    public float Smoothening;

    private void Update()
    {
        if (!Target)
            return;

        transform.position = Target.transform.position.SetZ(transform.position.z);
    }
}
