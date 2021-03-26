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
            transform.localScale = Vector3.Lerp(originalScaleVector/2, SmackMaxSize, SmackCurve.Evaluate(lerpTime));

            lerpTime += SmackSpeed;
            yield return new WaitForEndOfFrame();
        }
    }
}
