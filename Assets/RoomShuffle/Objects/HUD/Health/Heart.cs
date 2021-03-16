using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Heart : MonoBehaviour
{
    
    private Transform cake;
    private Image cakeImage;
    void Awake()
    {
        cake = transform.Find("Cake");
        cakeImage = cake.GetComponent<Image>();
    }

    /// <summary>
    /// Sets the position relative to the upper left corner of the HUD
    /// </summary>
    /// <param name="position"></param>
    public void setPosition(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>
    /// Make a cake diagram of given health from a float (0 to 1).
    /// </summary>
    /// <param name="health"></param>
    public void setHealth(float health)
    {
        cakeImage.fillAmount = health;
    }
}
