using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Causes an object to counteract the flipping effect on the camera
/// </summary>
public class DontFlip : MonoBehaviour
{
    /// <summary>
    /// The last flip state of the camera
    /// </summary>
    private bool _lastCameraFlipState;

    void Update()
    {
        if (FlipCamera.IsFlipped != _lastCameraFlipState)
        {
            //Scale object to -1x its current size
            _lastCameraFlipState = FlipCamera.IsFlipped;
            transform.localScale = transform.localScale.SetX(-transform.localScale.x);
        }
    }
}
