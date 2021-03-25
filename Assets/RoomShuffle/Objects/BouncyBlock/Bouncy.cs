using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [Tooltip("The minimum speed the block will push you in")]
    public float MinimumPushBackVelocity;
    
    [Tooltip("The amount your velocity will be multiplied with when holding jump")]
    public float HoldingJumpScale;

    [Tooltip("The amount of time the player will not have a speed cap")]
    public float NoSpeedCapTime;

    private GroundController _playerGroundController;
    private float _playerMaxSpeed;
    private float _noSpeedCapTimeLeft = 0;
    private JumpController _jumpController;

    private void Update()
    {
        //Adjust player maximum horizontal speed 
        if (_noSpeedCapTimeLeft > 0)
        {
            float percentage = ( _noSpeedCapTimeLeft / NoSpeedCapTime) * 10;
            _noSpeedCapTimeLeft -= Time.deltaTime;
            _playerGroundController.MaxSpeed = (_playerMaxSpeed * percentage) + _playerMaxSpeed;
        }
        else
        {
            _playerGroundController.MaxSpeed = _playerMaxSpeed;
        }
    }

    private void Start()
    {
        GameObject player = this.GetPlayer();
        _playerGroundController = player.GetComponent<GroundController>();
        _playerMaxSpeed = _playerGroundController.MaxSpeed;
        _jumpController = player.GetComponent<JumpController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 relativeVelocity = collision.relativeVelocity;
        
        if (contact.normal.x != 0)
        {
            //if the collision is with a player: disable the speed cap for a little while
            if (collision.gameObject == this.GetPlayer())
            {
                _noSpeedCapTimeLeft = NoSpeedCapTime;
            }

            int direction = Math.Sign(relativeVelocity.x);
            float pushBack = Mathf.Max(Math.Abs(relativeVelocity.x), MinimumPushBackVelocity);

            //Invert and add velocity
            collision.rigidbody.SetVelocityX(pushBack * -direction);
        }
        else
        {
            int direction = Math.Sign(relativeVelocity.y);
            float pushBack = Mathf.Max(Math.Abs(relativeVelocity.y), MinimumPushBackVelocity);
            
            //if the collision is with a player: check if the player is holding jump
            if (collision.gameObject == this.GetPlayer())
            {
                //If the player is hoding jump: multiply the verical speed
                if (Input.GetButton("Jump"))
                {
                    pushBack *= HoldingJumpScale;
                }
            }
            
            //Invert and add velocity
            collision.rigidbody.SetVelocityY(pushBack * -direction);
            
            _jumpController.CaptureSuccessfulJumpSnapshot();
        }
        
    }
}
