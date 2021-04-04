﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Causes an object to perpetually jump whenever it touches the ground
/// </summary>
public class PerpetualJump : MonoBehaviour
{
    [Tooltip("The force (speed) of the jump")]
    public float JumpForce = 5f;

    [Tooltip("How long the object must stay grounded before jumping again")]
    public float JumpInterval;

    [Header("Effect Config")]
    [Tooltip("If enabled, the enemies wait time is reduced when the fast-foe effect is enabled")]
    public bool AffectedByFastFoe;


    private IEnumerator Start()
    {
        var rigidbody = GetComponentInParent<Rigidbody2D>();

        while (true)
        {
            var time = 0f;
            var breakBecauseAirborne = false;
            var waitTime = JumpInterval;

            //Apply fast-foe effect
            if (AffectedByFastFoe)
                waitTime = Commons.GetEffectValue(waitTime, EffectValueType.EnemyWaitTime);

            //The object waits for its interval
            while (time <= waitTime)
            {
                //If while waiting the object becomes airborne. Reset the waiting
                if (!this.OnGround2D())
                {
                    breakBecauseAirborne = true;
                    break;
                }

                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            //Reset waiting
            if (breakBecauseAirborne)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
                
            //Waiting finished. Make the object jump
            rigidbody.SetVelocityY(JumpForce);
            yield return new WaitForEndOfFrame();
        }
    }
}