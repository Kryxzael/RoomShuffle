using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;



public class ItemUIManager : MonoBehaviour
{
    public Image _image;
    public Image _imageCopy;
    public TextMeshProUGUI _text;
    

    /// <summary>
    /// Sets the "durability left on the item. From 0 to 1"
    /// </summary>
    public void SetItemImageFill(float fillAmount)
    {
        _imageCopy.fillAmount = fillAmount;
    }

    /// <summary>
    /// Sets the text of the textmeshpro element
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        _text.text = text;
    }
}
