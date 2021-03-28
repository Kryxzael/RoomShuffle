using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Locks a camera to a boundary based on two points
/// </summary>
[RequireComponent(typeof(Camera))]
public class LockedCamera : MonoBehaviour
{
    private Camera _cam;
    private RenewableLazy<CameraStopPoint> _topLeftPoint = new RenewableLazy<CameraStopPoint>(() => FindObjectsOfType<CameraStopPoint>().SingleOrDefault(i => i.Corner == CameraStopPoint.CamStopCorner.TopLeft));
    private RenewableLazy<CameraStopPoint> _bottomRightPoint = new RenewableLazy<CameraStopPoint>(() => FindObjectsOfType<CameraStopPoint>().SingleOrDefault(i => i.Corner == CameraStopPoint.CamStopCorner.BottomRight));

    private void Awake()
    {
        //Note. This camera only really works in orthographic mode
        _cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (_topLeftPoint.Value == null || _bottomRightPoint.Value == null || Cheats.Noclip)
            return;

        Bounds currentBounds = GetCameraBounds(_cam);

        var distanceLeft = _topLeftPoint.Value.transform.position.x - currentBounds.min.x;
        var distanceBottom = _bottomRightPoint.Value.transform.position.y - currentBounds.min.y;

        var distanceTop = currentBounds.max.y - _topLeftPoint.Value.transform.position.y;
        var distanceRight = currentBounds.max.x - _bottomRightPoint.Value.transform.position.x;

        if (distanceLeft > 0)
            transform.position += Vector3.right * distanceLeft;

        if (distanceTop > 0)
            transform.position += Vector3.down * distanceTop;

        if (distanceRight > 0)
            transform.position += Vector3.left * distanceRight;

        if (distanceBottom > 0)
            transform.position += Vector3.up * distanceBottom;
    }


    /// <summary>
    /// Gets the boundary of the camera when in orthographic mode
    /// </summary>
    /// <returns></returns>
    public static Bounds GetCameraBounds(Camera cam)
    {
        var screenAspect = (float)Screen.width / Screen.height;
        var cameraHeight = cam.orthographicSize * 2f;

        return new Bounds(cam.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight));

        //Source (Adapted): https://answers.unity.com/questions/501893/calculating-2d-camera-bounds.html by GeekyMonkey 28.3.21
    }
}
