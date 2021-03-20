using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

/// <summary>
/// A UI element that describes <see cref="HealthController.HP_PER_HEART" /> of health
/// </summary>
public class Heart : MonoBehaviour
{
    //The image that will be the cake or pie of the heart, representing the fullness of the heart
    private Image cakeImage;

    void Awake()
    {
        cakeImage = transform.Find("Cake").GetComponent<Image>();
    }

    /// <summary>
    /// Make a cake diagram of given health from a float (0 to 1).
    /// </summary>
    /// <param name="percentage"></param>
    public void SetHeartFillPercentage(float percentage)
    {
        cakeImage.fillAmount = percentage;
    }
}
