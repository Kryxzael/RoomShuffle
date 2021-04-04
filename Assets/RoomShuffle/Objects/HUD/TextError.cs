using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextError : MonoBehaviour
{
    public float LerpSpeed;

    [Tooltip("How long the element wil sway")]
    public float swayingLength;
    
    public AnimationCurve SwayCurve;

    public AnimationCurve SmackCurve;

    private TextMeshProUGUI TMP;

    //TODO remove the nonsense in update... or just the whole update method
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ErrorLerp();
        }
    }

    public void Awake()
    {
        TMP = GetComponent<TextMeshProUGUI>();
    }

    public void ErrorLerp()
    {
        StartCoroutine(CoErrorLerp());
    }

    private IEnumerator CoErrorLerp()
    {
        float lerpTime = 0f;
        while (lerpTime < 1f)
        {
            transform.localPosition = Vector2.Lerp(Vector2.zero, new Vector2(swayingLength, 0), SwayCurve.Evaluate(lerpTime));

            TMP.color = Color.Lerp(Color.white, Color.red, SmackCurve.Evaluate(lerpTime));
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, SmackCurve.Evaluate(lerpTime));

            lerpTime += LerpSpeed * Time.deltaTime * 240f;
            yield return new WaitForEndOfFrame();
        }

        TMP.color = Color.white;
    }
}