using System.Collections;

using TMPro;
using UnityEngine;

/// <summary>
/// A script, meant for the currency counter, to make a denied animation and sound
/// </summary>
public class TextError : MonoBehaviour
{
    public float LerpSpeed;

    [Tooltip("How long the element wil sway")]
    public float swayingLength;
    
    [Tooltip("How much the object will swat from right to left")]
    public AnimationCurve SwayCurve;

    [Tooltip("How big the object will be at any time")]
    public AnimationCurve SmackCurve;

    private TextMeshProUGUI TMP;

    private MultiSoundPlayer _multiSoundPlayer;

    public void Awake()
    {
        //Get text object
        TMP = GetComponent<TextMeshProUGUI>();

        //get soundplayer
        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();
    }

    /// <summary>
    /// Makes a "denied" -sound and makes a swaying animation on the text object
    /// </summary>
    public void ErrorLerp()
    {
        PlayErrorSound();
        StartCoroutine(CoErrorLerp());
    }

    /// <summary>
    /// Lerps scale, color and position 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoErrorLerp()
    {
        float lerpTime = 0f;
        while (lerpTime < 1f)
        {
            
            transform.localPosition = Vector2.Lerp(Vector2.zero, new Vector2(swayingLength, 0), SwayCurve.Evaluate(lerpTime));

            TMP.color = Color.Lerp(Color.white, Color.red, SmackCurve.Evaluate(lerpTime));
            
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, SmackCurve.Evaluate(lerpTime));

            /*
             * So about '240f'. Here's a story about the TextSmack script.
             * At its inception, SmackSpeed was not multiplied by delta-time.
             * When we found out that this coroutine was frame-dependent, we fixed it
             * But in doing so, all smacks in the entire game suddenly became very slow
             * Therefore, we decided that the easiest solution was to multiply the value
             * by our average frame rate (240) to keep the intended speed
             */
            lerpTime += LerpSpeed * Time.deltaTime * 240f;
            
            yield return new WaitForEndOfFrame();
        }

        //set color back to white after animation
        TMP.color = Color.white;
    }

    private void PlayErrorSound()
    {
        _multiSoundPlayer.PlaySound(2);
    }
}