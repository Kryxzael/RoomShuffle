using System;
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
        _renderer = GetComponentInParent<SpriteRenderer>();
    }

    public Hitbox()
    {        
    }

    /// <summary>
    /// Enables invincibility time for the object
    /// </summary>
    public void GrantInvincibilityFrames()
    {
        StopAllCoroutines();

        //God players do not have invincibility frames
        if (Cheats.HealthCheat == Cheats.HealthCheatType.Godmode && this is PlayerHitbox)
            return;

        StartCoroutine(CoIFrames());
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
        const float BLINK_RATE = 0.1f;

        while (elapsedTime < InvincibilityFramesInSeconds)
        {
            //Swap blink state
            isBlinking = !isBlinking;

            //Set opacity of sprite in accordance with current blinking state
            if (_renderer != null)
                _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, isBlinking ? 0f : 1f);

            //Wait for frame to end and increase counter
            yield return new WaitForSecondsRealtime(BLINK_RATE);
            elapsedTime += BLINK_RATE;
        }

        //Make sure the sprite is visible again when the blinking ends
        if (_renderer != null)
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1f);

        //Set mortal
        HasInvincibilityFrames = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        object x = "Hello";

        if (collision.GetComponent<HurtBox>() is HurtBox hurt && hurt)
            TryDealDamageBy(hurt);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<HurtBox>() is HurtBox hurt && hurt && hurt.ContinuousDamage)
            TryDealDamageBy(hurt);
    }

    /// <summary>
    /// Lets the provided hurtbox damage the hitbox
    /// </summary>
    /// <param name="hurtbox"></param>
    public void TryDealDamageBy(HurtBox hurtbox)
    {
        if ((HasInvincibilityFrames && !hurtbox.IgnoresInvincibilityFrames) || !hurtbox.GetTargets().HasFlag(SusceptibleTo))
            return;

        hurtbox.OnDealDamage(this);
        OnReceiveDamage(hurtbox);
    }

    protected abstract void OnReceiveDamage(HurtBox hurtbox);
}
