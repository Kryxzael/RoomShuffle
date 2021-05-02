using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ItemUIManager : MonoBehaviour
{
    public Image Image;
    public Image ImageCopy;
    public TextMeshProUGUI Text;

    /// <summary>
    /// Sets the progress (cooldown, power-up timer, etc.) displayed on the item. From 0 to 1
    /// </summary>
    public void SetItemImageFill(float fillAmount)
    {
        ImageCopy.fillAmount = fillAmount;
    }

    /// <summary>
    /// Sets the text of the textmeshpro element
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        Text.text = text;
    }
}
