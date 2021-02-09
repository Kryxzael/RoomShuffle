using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Lets an object smoothly follow another object
/// </summary>
public class LerpToTransform : MonoBehaviour
{
    /// <summary>
    /// Gets the target to follow
    /// </summary>
    [Tooltip("The transform the object will follow")]
    public Transform Target;

    [Header("Sharpness")]
    [Range(0f, 1f)]
    [Tooltip("How sharply the object will follow the target on a horizontal plane")]
    public float HorizontalSharpness = 0.5f;

    [Range(0f, 1f)]
    [Tooltip("How sharply the object will follow the target on a vertical plane")]
    public float VerticalSharpness = 0.1f;

    [Header("Offset")]
    [Tooltip("How many world units to offset the camera by upwards when the player is looking up")]
    public float LookUpOffset = 2.5f;

    [Tooltip("How many world units to offset the camera by downwards when the player is looking down")]
    public float LookDownOffset = 2.5f;

    private void FixedUpdate()
    {
        float offset = 0f;

        if (Input.GetAxisRaw("Vertical") < -0.25f)
            offset = LookUpOffset;
        else if (Input.GetAxisRaw("Vertical") > 0.25f)
        {
            offset = -LookDownOffset;
        }

        transform.position = new Vector3(
            x: Mathf.Lerp(transform.position.x, Target.transform.position.x, HorizontalSharpness),
            y: Mathf.Lerp(transform.position.y, Target.transform.position.y + offset, VerticalSharpness),
            z: transform.position.z
        );
    }
}
