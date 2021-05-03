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
    [Range(-1f, 1f)]
    public float ParalaxDepth = 1f;

    /* *** */

    private static RenewableLazy<Camera> _camera = new RenewableLazy<Camera>(() => Camera.main);
    private Vector3? _lastCameraPosition;


    private void LateUpdate()
    {
        /*
         * Set layer position based on camera position
         */

        var cam = _camera.Value;

        if (!cam)
            return;

        if (_lastCameraPosition == null)
        {
            _lastCameraPosition = cam.transform.position;
            return;
        }

        transform.Translate((cam.transform.position - _lastCameraPosition.Value) * ParalaxDepth);
        _lastCameraPosition = cam.transform.position;
    }


}
