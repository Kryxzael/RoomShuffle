using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Makes an object (background) snap to the bottom left corner of the room that is loaded
/// </summary>
public class SnapToBottomLeftCornerOfRoom : MonoBehaviour
{
    private void Start()
    {
        var anchorTopLeft = FindObjectsOfType<CameraStopPoint>().SingleOrDefault(i => i.Corner == CameraStopPoint.CamStopCorner.TopLeft);
        var anchorBottomRight = FindObjectsOfType<CameraStopPoint>().SingleOrDefault(i => i.Corner == CameraStopPoint.CamStopCorner.BottomRight);

        if (!anchorBottomRight || !anchorTopLeft)
        {
            Debug.LogWarning("Background parallax layer cannot be aligned because the current room is missing its camera stop points");
        }

        //This assumes that the sprite is anchored at the bottom left corner
        transform.position = new Vector3(anchorTopLeft.transform.position.x, anchorBottomRight.transform.position.y, 0f);
    }
}
