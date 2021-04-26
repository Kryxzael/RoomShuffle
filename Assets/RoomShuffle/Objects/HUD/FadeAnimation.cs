using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : MonoBehaviour
{
    public Image FadeScreen;
    public Mask FadeMask;

    public float FadeTime = 0.5f;

    public void BeginFade(Func<IEnumerator> onBetweenFades, Func<IEnumerator> onBeforeFade = null, Func<IEnumerator> onAfterFade = null)
    {
        StartCoroutine(CoFade());

        IEnumerator CoFade()
        {
            if (onBeforeFade != null)
                yield return onBeforeFade();

            /*
             * Fade out
             */

            for (float i = FadeMask.transform.localScale.x; i > 0f; i -= Time.unscaledDeltaTime / FadeTime)
            {
                FadeMask.transform.localScale = new Vector3(i, i, 1f);
                yield return new WaitForEndOfFrame();
            }

            FadeMask.transform.localScale = new Vector3(0.0001f, 0.0001f, 1f);

            /*
             * Execute handler
             */

            yield return onBetweenFades();

            /*
             * Fade in
             */
            for (float i = FadeMask.transform.localScale.x; i <= 1f; i += Time.unscaledDeltaTime / FadeTime)
            {
                FadeMask.transform.localScale = new Vector3(i, i, 1f);
                yield return new WaitForEndOfFrame();
            }

            FadeMask.transform.localScale = Vector3.one;

            if (onAfterFade != null)
                yield return onAfterFade();
        }
    }


}
