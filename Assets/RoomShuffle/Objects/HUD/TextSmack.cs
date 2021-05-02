using System.Collections;
using UnityEngine;

/// <summary>
/// Makes an objects scale lerp with a curve
/// </summary>
public class TextSmack : MonoBehaviour
{
    [Tooltip("Hpw fast the object should lerp")]
    public float SmackSpeed;
    
    [Tooltip("The maximum size the object will lerp to")]
    public Vector3 SmackMaxSize = Vector3.one * 2;
    
    [Tooltip("The curve the lerp will follow")]
    public AnimationCurve SmackCurve;

    public void Smack(float originalScale = 1)
    {
        Vector2 originalScaleVector = new Vector2(originalScale, originalScale);
        StartCoroutine(CoSmack(originalScaleVector));
    }

    private IEnumerator CoSmack(Vector2 originalScaleVector)
    {
        float lerpTime = 0f;
        while (lerpTime < 1f)
        {
            transform.localScale = Vector3.Lerp(new Vector2(0,0), SmackMaxSize, SmackCurve.Evaluate(lerpTime));

            /*
             * So about '240f'. Here's a story about the TextSmack script.
             * At its inception, SmackSpeed was not multiplied by delta-time.
             * When we found out that this coroutine was frame-dependent, we fixed it
             * But in doing so, all smacks in the entire game suddenly became very slow
             * Therefore, we decided that the easiest solution was to multiply the value
             * by our average frame rate (240) to keep the intended speed
             */
            
            lerpTime += SmackSpeed * Time.deltaTime * 240f;
            yield return new WaitForEndOfFrame();
        }

        //set the scale to the original scale after the animation
        transform.localScale = new Vector3(originalScaleVector.x, originalScaleVector.y, originalScaleVector.x);
    }
}
