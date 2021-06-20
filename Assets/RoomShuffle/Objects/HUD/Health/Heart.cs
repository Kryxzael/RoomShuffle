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
    public Sprite ContainerImageNormal;
    public Sprite FillImageNormal;

    public Sprite ContainerImageDanger;
    public Sprite FillImageDanger;

    //The image that will be the cake or pie of the heart, representing the fullness of the heart
    //The names are outdated as we use batteries instead of cakes
    private Image cakeImage;
    private Image containerImage;

    //The smack component
    private TextSmack _smack;

    void Awake()
    {
        containerImage = transform.Find("Background").GetComponent<Image>();
        cakeImage = transform.Find("Cake").GetComponent<Image>();
    }

    private void Start()
    {
        //start smack animation on start
        _smack = GetComponent<TextSmack>();
        _smack.Smack();
    }

    private void Update()
    {
        //if the player is near death, set the container images to the "danger" variant
        if (Commons.PlayerHealth.Health - Commons.PlayerHealth.GetSoftDeathDamage() <= 0)
        {
            containerImage.sprite = ContainerImageDanger;
            cakeImage.sprite = FillImageDanger;


            const float PULSE_SPEED = 10f;
            const float PULSE_DEPTH = 0.7f;
            containerImage.color = cakeImage.color = Color.Lerp(Color.white, new Color(PULSE_DEPTH, PULSE_DEPTH, PULSE_DEPTH), Mathf.Sin(Time.unscaledTime * PULSE_SPEED));
        }
        else
        {
            containerImage.sprite = ContainerImageNormal;
            cakeImage.sprite = FillImageNormal;

            containerImage.color = cakeImage.color = Color.white;
        }
    }

    /// <summary>
    /// Make a cake diagram of given health from a float (0 to 1).
    /// </summary>
    /// <param name="percentage"></param>
    public void SetHeartFillPercentage(float percentage)
    {
        //return if the health hasn't changed
        if (cakeImage.fillAmount == percentage)
            return;

        //set fill amount
        cakeImage.fillAmount = percentage;
    }
}
