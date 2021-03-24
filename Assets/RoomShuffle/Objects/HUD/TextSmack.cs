using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSmack : MonoBehaviour
{
    public float SmackSpeed;
    public Vector3 SmackMaxSize = Vector3.one * 2;
    public AnimationCurve SmackCurve;
    
    public IEnumerator Smack()
    {
        float lerpTime = 0f;
        while (lerpTime < 1f)
        {
            transform.localScale = Vector3.Lerp(Vector2.one/2, SmackMaxSize, SmackCurve.Evaluate(lerpTime));

            lerpTime += SmackSpeed;
            yield return new WaitForEndOfFrame();
        }
    }
}
