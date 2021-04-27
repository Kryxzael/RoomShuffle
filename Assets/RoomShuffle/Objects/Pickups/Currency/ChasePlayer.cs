using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Makes an object chase the player
/// </summary>
public class ChasePlayer : MonoBehaviour
{
    public AnimationCurve SpeedCurve;

    public float CurveMaxSpeed;
    public float CurveMaxTime;

    public void BeginChase()
    {
        StartCoroutine(coChase());

        IEnumerator coChase()
        {
            var target = this.GetPlayer().transform;
            var time = 0f;

            while (true)
            {
                var direction = (target.position - transform.position).normalized;
                transform.position += direction * SpeedCurve.Evaluate(Mathf.Min(1f, time / CurveMaxTime)) * CurveMaxSpeed;

                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
        }
    }
}
