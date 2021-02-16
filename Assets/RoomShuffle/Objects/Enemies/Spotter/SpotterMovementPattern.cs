using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

/// <summary>
/// An enemy pattern that can spot the player and chase them
/// </summary>
[RequireComponent(typeof(Flippable), typeof(SpotPlayer), typeof(Rigidbody2D))]
public class SpotterMovementPattern : MonoBehaviour
{
    [Tooltip("The speed the spotter will move at when fumbling")]
    public float WalkSpeed;

    [Tooltip("The speed the spotter will move at when chasing")]
    public float RunSpeed;

    [Header("Fumbling Wait Times")]
    [Tooltip("The minimum amount of time the spotter can move or wait before choosing a new action when fumbling")]
    public float MinFumbleWaitTime;

    [Tooltip("The maximum amount of time the spotter can move or wait before choosing a new action when fumbling")]
    public float MaxFumbleWaitTime;
    
    //The remaining fumble wait time
    private float _fumbleCurrentWaitTime;

    //If the current fumble mode is walking
    private bool _fumbleWalk = true;

    /* *** */

    private Rigidbody2D _rigid;
    private GameObject _player;
    private Flippable _flippable;
    private SpotPlayer _spotPlayer;

    void Start()
    {
        _player = this.GetPlayer();

        _rigid = GetComponent<Rigidbody2D>();
        _flippable = GetComponent<Flippable>();
        _spotPlayer = GetComponent<SpotPlayer>();
    }
    
    void Update()
    {
        switch (_spotPlayer.State)
        {
            case SpotterPlayerRelationship.OutOfRadius:
            case SpotterPlayerRelationship.HiddenInRadius:   
                FumbleAround();
                break;

            case SpotterPlayerRelationship.Spotted:
                FlipToPlayer();
                _rigid.SetVelocityX(0f);
                break;

            case SpotterPlayerRelationship.Puzzled:
                _rigid.SetVelocityX(0f);
                break;

            case SpotterPlayerRelationship.Chasing:
                FlipToPlayer();
                _rigid.SetVelocityX(_flippable.DirectionSign * RunSpeed);
                break;
        }
    }

    /// <summary>
    /// Flips the object around to face the player
    /// </summary>
    private void FlipToPlayer()
    {
        int relativePositionSign = Math.Sign(_player.transform.position.x - transform.position.x);

        if (_flippable.DirectionSign != relativePositionSign)
            _flippable.Flip();
    }
    
    /// <summary>
    /// Makes the object move around randomly
    /// </summary>
    private void FumbleAround()
    {
        _fumbleCurrentWaitTime -= Time.deltaTime;

        //Fumbling
        if (_fumbleWalk)
        { 
            _rigid.SetVelocityX(_flippable.DirectionSign * WalkSpeed);
        }

        //Stationary
        else
        {
            _rigid.SetVelocityX(0f);
        }

        //Choose a new action
        if (_fumbleCurrentWaitTime <= 0)
        {
            if (!_fumbleWalk && Random.Range(0, 2) == 0)
                _flippable.Flip();

            _fumbleWalk = !_fumbleWalk;
            _fumbleCurrentWaitTime = Random.Range(MinFumbleWaitTime, MaxFumbleWaitTime);
        }
    }
}