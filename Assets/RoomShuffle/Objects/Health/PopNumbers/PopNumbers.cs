using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopNumbers : MonoBehaviour
{
    public float FallSpeed;
    public Vector3 SmackMaxSize = Vector3.one * 2;
    public AnimationCurve SmackCurve;

    private TextMeshPro TMP;

    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;
        TMP = GetComponent<TextMeshPro>();
        PopNumber();
    }

    public void PopNumber(float originalScale = 1)
    {
        Vector2 originalScaleVector = new Vector2(originalScale, originalScale);
        StartCoroutine(CoPopNumber(originalScaleVector));
    }

    private IEnumerator CoPopNumber(Vector2 originalScaleVector)
    {
        float lerpTime = 1f;
        while (lerpTime > 0f)
        {
            transform.localScale = Vector3.Lerp(originalScaleVector/2, SmackMaxSize, SmackCurve.Evaluate(lerpTime));
            transform.position = Vector3.Lerp(_originalPosition + Vector3.up, _originalPosition + (Vector3.up*2.5f),
                SmackCurve.Evaluate(lerpTime));

            TMP.color = Color.Lerp(Color.red, new Color(255, 0, 0, 0), SmackCurve.Evaluate(lerpTime));

            lerpTime -= FallSpeed;
            yield return new WaitForEndOfFrame();
        }
        
        Destroy(gameObject);
    }
}