using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Creates a label that moves, changes color and scale before being destroyed
/// </summary>
public class PopNumber : MonoBehaviour
{
    [Header("End Values")]
    [Tooltip("The scale the label with have at the end of its lifetime")]
    public Vector3 EndSize = Vector3.one * 2f;

    [Tooltip("The positional offset relative to its spawn point the label will have at the end of its lifetime")]
    public Vector3 RelativeEndPosition;

    [Tooltip("The color the label will have at the end of its lifetime")]
    public Color EndColor;

    [Header("Animation")]
    [Tooltip("How long the animation will last for")]
    public float AnimationLength = 5f;

    [Tooltip("The curve used to lerp the values")]
    public AnimationCurve AnimationCurve;

    /* *** */

    private TextMeshPro _textMeshPro;
    private TextMeshProUGUI _textMeshProUGUI;
    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;
        _textMeshPro = GetComponent<TextMeshPro>();

        if (!_textMeshPro)
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            StartCoroutine(CoPopNumber(transform.position, transform.localScale, _textMeshProUGUI.color));
        }
        else
        {
            StartCoroutine(CoPopNumber(transform.position, transform.localScale, _textMeshPro.color));
        }
    }

    private IEnumerator CoPopNumber(Vector3 startPosition, Vector3 startSize, Color startColor)
    {
        float lerpTime = 0f;
        while (lerpTime < AnimationLength)
        {
            transform.localScale = Vector3.Lerp(
                a: startSize, 
                b: EndSize, 
                t: AnimationCurve.Evaluate(lerpTime)
            );

            //Override for flipped camera
            transform.localScale = transform.localScale.SetX(transform.localScale.x * (FlipCamera.IsFlipped ? -1f : 1f));

            transform.position = Vector3.Lerp(
                a: startPosition, 
                b: _originalPosition + RelativeEndPosition,
                t: AnimationCurve.Evaluate(lerpTime)
            );

            if (!_textMeshPro)
            {
                _textMeshProUGUI.color = Color.Lerp(startColor, EndColor, AnimationCurve.Evaluate(lerpTime));
            }
            else
            {
                _textMeshPro.color = Color.Lerp(startColor, EndColor, AnimationCurve.Evaluate(lerpTime));
            }
            

            lerpTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        Destroy(gameObject);
    }
}