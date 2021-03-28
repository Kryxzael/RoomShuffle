using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Tells the camera not to go past a certain point
/// </summary>
public class CameraStopPoint : MonoBehaviour
{
    [Tooltip("The corner the cam stop point is at")]
    public CamStopCorner Corner;

    /// <summary>
    /// The corner the cam stop point is at
    /// </summary>
    public enum CamStopCorner
    {
        TopLeft,
        BottomRight
    }
}
