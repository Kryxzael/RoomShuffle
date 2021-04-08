using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UnityEngine;

//Adapted, original at: https://answers.unity.com/questions/20337/flipmirror-camera.html by videordealmeida at Jan 17 2017 03:30 PM

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class FlipCamera : MonoBehaviour
{
    /// <summary>
    /// Whether the camera is currently flipped
    /// </summary>
    public static bool IsFlipped { get; set; } = false;

    new private Camera camera;

    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void OnPreCull()
    {
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(IsFlipped ? -1 : 1, 1, 1);
        camera.projectionMatrix *= Matrix4x4.Scale(scale);
    }
    void OnPreRender()
    {
        GL.invertCulling = IsFlipped;
    }

    void OnPostRender()
    {
        GL.invertCulling = false;
    }
}