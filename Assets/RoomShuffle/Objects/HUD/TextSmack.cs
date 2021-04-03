using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSmack : MonoBehaviour
{
    public float SmackSpeed;
    public Vector3 SmackMaxSize = Vector3.one * 2;
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
            transform.localScale = Vector3.Lerp(originalScaleVector / 2, SmackMaxSize, SmackCurve.Evaluate(lerpTime));

            /*
             * So about '240f'. Here's a story about the TextSmack script.
             * At its inception, SmackSpeed was not multiplied by delta-time.
             * When we found out that this coroutine was frame-dependent, we fixed it
             * But in doing so, all smacks in the entire game suddenly became very slow
             * Therefore, we decided that the easiest (laziest) solution was to multiply the value
             * by our average frame rate (240) to keep the intended speed
             * 
             * Also: 
             * TODO I guess?
             */
            
            lerpTime += SmackSpeed * Time.deltaTime * 240f;
            yield return new WaitForEndOfFrame();
        }
    }
}
