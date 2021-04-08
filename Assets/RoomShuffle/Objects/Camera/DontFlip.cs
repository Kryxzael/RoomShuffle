using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Causes an object to counteract the flipping effect on the camera
/// </summary>
public class DontFlip : MonoBehaviour
{
    private bool _lastCameraFlipState;

    // Update is called once per frame
    void Update()
    {
        if (FlipCamera.IsFlipped != _lastCameraFlipState)
        {
            _lastCameraFlipState = FlipCamera.IsFlipped;
            transform.localScale = transform.localScale.SetX(-transform.localScale.x);
        }
    }
}
