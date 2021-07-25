using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the downfall generator attached to the camera
/// </summary>
public class DownfallController : MonoBehaviour
{
    private void Start()
    {
        var child = transform.Find(Commons.RoomGenerator.CurrentRoomConfig.Theme.ToString());

        foreach (Transform i in transform)
        {
            if (i == child)
                continue;

            i.gameObject.SetActive(false);
        }
    }
}
