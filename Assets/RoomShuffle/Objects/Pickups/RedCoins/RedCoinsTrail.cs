using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
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

    private int _lastNumberOfChildren;

    public GameObject PlayerTimerPrefab;

    private GameObject PlayerTimerInstance;

    private void Awake()
    {

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

    private void Start()
    {
        //Find the original number of children
        _lastNumberOfChildren = transform.Cast<Transform>().Count();
        
        //Create countdown timer instance on player
        GameObject player = this.GetPlayer();
        PlayerTimerInstance = Instantiate(PlayerTimerPrefab, player.transform.position + Vector3.up, quaternion.identity, player.transform);

        //get timer component
        _countdownTimer = PlayerTimerInstance.GetComponent<Timer>();
    }

    private void Update()
    {
        //Try to start coundown. This returns if it has already started
        _countdownTimer.StartCountdown(CountDownTime);
        
        _timePassed += Time.deltaTime;

        //If timer how reached zero: disable trail and destroy timer object
        if (_countdownTimer.CurrentSeconds <= 0f)
        {
            gameObject.SetActive(false);
        }

        //Compare number of children to last frame
        int currentNumberOfChildren = transform.Cast<Transform>().Count();
        if (currentNumberOfChildren < _lastNumberOfChildren)
        {
            _lastNumberOfChildren = currentNumberOfChildren;
            
            //when a pickup has been picked up: make sound
            //TODO Make pickup sound
        }

        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            //If the spriterenderer doesn't exit (Because it's been picked up) continue
            if (!spriteRenderer)
            {
                continue;
            }

            //Make the sprites blink
            spriteRenderer.color = Color.Lerp(Color.red, new Color(1f, 0f, 0f, 0f), (Mathf.Cos(_timePassed * 8) + 1) / 2);
        }

    }
    

    /// <summary>
    /// Destroy the timer when the trail group og the chain gets disabled. (Because the player didn't pick up the items fast enough)
    /// </summary>
    private void OnDisable()
    {
        Destroy(PlayerTimerInstance);
    }

    //The player successfully picked up all items.
    private void OnDestroy()
    {
        Destroy(PlayerTimerInstance);
        
        //TODO Make final pickup sound
    }
}
