using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Allows a background layer to parallax with the camera
/// </summary>
public class Parallax : MonoBehaviour
{
    //The camera to follow
    private static RenewableLazy<Camera> _camera = new RenewableLazy<Camera>(() => Camera.main);

    //The camera's position last frame
    private Vector3? _lastCameraPosition;

    /* *** */

    [Tooltip("The perceived depth of the object. 1 = Always follows camera. 0 = Normal object")]
    [Range(-1f, 1f)]
    public float ParalaxDepth = 1f;


    private void LateUpdate()
    {
        var cam = _camera.Value;

        //If no camera's are rendering, stop. This can happen in level transitions.
        if (!cam)
            return;

        //If this is the first frame, calibrate parallax here.
        if (_lastCameraPosition == null)
        {
            _lastCameraPosition = cam.transform.position;
            return;
        }

        //Offset object based on camera's motion
        transform.Translate((cam.transform.position - _lastCameraPosition.Value) * ParalaxDepth);
        _lastCameraPosition = cam.transform.position;
    }


}
