using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : MonoBehaviour
{
    public RawImage FadeScreen;

    public float FadeTime = 0.5f;

    public void BeginFade(Func<IEnumerator> onBetweenFades)
    {
        StartCoroutine(CoFade());

        IEnumerator CoFade()
        {
            var color = FadeScreen.color;

            /*
             * Fade out
             */

            for (float i = FadeScreen.color.a; i <= 1f; i += Time.deltaTime / FadeTime)
            {
                color.a = i;
                FadeScreen.color = color;
                yield return new WaitForEndOfFrame();
            }

            color.a = 1f;
            FadeScreen.color = color;

            /*
             * Execute handler
             */

            yield return onBetweenFades();

            /*
             * Fade in
             */
            for (float i = FadeScreen.color.a; i >= 0f; i -= Time.deltaTime / FadeTime)
            {
                color.a = i;
                FadeScreen.color = color;
                yield return new WaitForEndOfFrame();
            }

            color.a = 0f;
            FadeScreen.color = color;
        }
    }


}
