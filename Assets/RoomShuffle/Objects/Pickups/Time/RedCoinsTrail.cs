using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCoinsTrail : MonoBehaviour
{
    [Tooltip("The amount of seconds to add to the timer")]
    public float CountDownTime;

    [Tooltip("The sprite that will highlight the trail objects")]
    public Sprite CircleSprite;

    private Timer _countdownTimer;
    private float _timePassed;
    
    private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        //Find countdowntimer
        _countdownTimer = Commons.RedCoinsCountdownTimer.GetComponent<Timer>();

        foreach (Transform child in transform)
        {
            //create child to child
            GameObject circleObject = new GameObject();
            circleObject.transform.parent = child;
            circleObject.transform.position = child.position;
            
            //Add spriterenderer
            SpriteRenderer spriteRenderer = circleObject.AddComponent<SpriteRenderer>();
            
            //Set correct sprite
            spriteRenderer.sprite = CircleSprite;
            spriteRenderer.color = Color.red;
            _spriteRenderers.Add(spriteRenderer);
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        _timePassed += Time.deltaTime;

        //If timer how reached zero: destroy trail
        if (_countdownTimer.CurrentSeconds <= 0f)
        {
            _countdownTimer.HideTimer();
            gameObject.SetActive(false);
        }

        //Make the sprites blink
        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            sr.color = Color.Lerp(Color.red, new Color(1f, 0f, 0f, 0f), (Mathf.Cos(_timePassed * 8) + 1) / 2);
        }

    }

    /// <summary>
    /// Starts a timer when the trail group og the chain gets enabled
    /// </summary>
    private void OnEnable()
    {
        _countdownTimer.ResetCountdown(CountDownTime);
    }

    /// <summary>
    /// Ends the timer when the trail group og the chain gets disabled
    /// </summary>
    private void OnDisable()
    {
        _countdownTimer.HideTimer();
    }

    private void OnDestroy()
    {
        _countdownTimer.HideTimer();
    }
}
