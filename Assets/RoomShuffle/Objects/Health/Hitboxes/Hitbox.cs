﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Lets an object detect collisions with hurtboxes
/// </summary>
public abstract class Hitbox : MonoBehaviour
{
    private SpriteRenderer _renderer; //Optional

    [Tooltip("The amount of seconds the object will be invincible")]
    public float InvincibilityFramesInSeconds = 5f;

    /// <summary>
    /// Gets the type of hurtboxes this hitbox is susceptible to
    /// </summary>
    public abstract HurtBoxTypes SusceptibleTo { get; }

    /// <summary>
    /// Gets whether the hitbox is currently under influence of I-frames
    /// </summary>
    public bool HasInvincibilityFrames { get; private set; }

    protected virtual void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Coroutine: Applies invincibility frames
    /// </summary>
    private IEnumerator CoIFrames()
    {
        //Set invincible
        HasInvincibilityFrames = true;

        //Store elapsed time of I-frames, and whether the object is currently blinking
        bool isBlinking = false;
        float elapsedTime = 0f;

        while (elapsedTime < InvincibilityFramesInSeconds)
        {
            //Swap blink state
            isBlinking = !isBlinking;

            //Set opacity of sprite in accordance with current blinking state
            if (_renderer != null)
                _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, isBlinking ? 0f : 1f);

            //Wait for frame to end and increase counter
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        //Make sure the sprite is visible again when the blinking ends
        if (_renderer != null)
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, isBlinking ? 0f : 1f);

        //Set mortal
        HasInvincibilityFrames = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (HasInvincibilityFrames)
            return;

        if (other.GetComponent<HurtBox>() is HurtBox hurt && hurt.Type.HasFlag(SusceptibleTo))
            OnReceiveDamage(hurt);
    }

    protected abstract void OnReceiveDamage(HurtBox hurtbox);
}
