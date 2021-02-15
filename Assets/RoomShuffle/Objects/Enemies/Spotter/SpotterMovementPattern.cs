using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpotterMovementPattern : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float minWaitTime;
    public float maxWaitTime;
    
    private Rigidbody2D _rigid;
    private GameObject _player;
    private float reactionTimeLeft;
    private float waitTime;
    private bool walk = true;
    private Flippable _flippable;
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _player = CommonExtensions.GetPlayer();
        _flippable = GetComponent<Flippable>();
    }
    
    void Update()
    {
        int relativePosition = Math.Sign(_player.transform.position.x - transform.position.x);
        
        switch (GetComponent<SpotPlayer>().epr)
        {
            case EnemyPlayerRelationship.outOfSight:
            case EnemyPlayerRelationship.inDistance:   
                FumbleAround();
                break;
            case EnemyPlayerRelationship.inSight:
                if (Math.Sign(_flippable.DirectionSign) != relativePosition)
                {
                    _flippable.Flip();
                }
                _rigid.velocity = new Vector2(0, _rigid.velocity.y);
                break;
            case EnemyPlayerRelationship.puzzled:
                _rigid.velocity = new Vector2(0, _rigid.velocity.y);
                break;
            case EnemyPlayerRelationship.chasing:
                if (Math.Sign(_flippable.DirectionSign) != relativePosition)
                {
                    _flippable.Flip();
                }
                _rigid.SetVelocityX(_flippable.DirectionSign * runSpeed);
                break;
        }
    }
    
    void FumbleAround()
    {
        if (walk)
        { //Fumbling

            _rigid.SetVelocityX(_flippable.DirectionSign * walkSpeed);

            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                walk = !walk;
                waitTime = Random.Range(minWaitTime, maxWaitTime);
            }
        }
        else
        { //Stationary
            _rigid.velocity = new Vector2(0,_rigid.velocity.y);
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                if (Random.Range(0, 2) == 1)
                {
                    _flippable.Flip();
                }

                walk = !walk;
                waitTime = Random.Range(minWaitTime, maxWaitTime);
            }
        }     
    }
}