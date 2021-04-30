using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RoomShuffle.Defaults;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [Tooltip("The minimum speed the block will push you in")]
    public float MinimumPushBackVelocity;
    
    [Tooltip("The amount your velocity will be multiplied with when holding jump")]
    public float HoldingJumpScale;

    [Tooltip("The amount of time the player will not have a speed cap")]
    public float NoSpeedCapTime;

    private float _playerMaxSpeed;
    private float _noSpeedCapTimeLeft = 0;
    private Camera _mainCamera;

    private MultiSoundPlayer _multiSoundPlayer;

    private RenewableLazy<GroundController> _playerGroundController = new RenewableLazy<GroundController>(() => CommonExtensions.GetPlayer().GetComponent<GroundController>());
    private RenewableLazy<JumpController> _jumpController = new RenewableLazy<JumpController>(() => CommonExtensions.GetPlayer().GetComponent<JumpController>());


    private void Start()
    {
        _playerMaxSpeed = _playerGroundController.Value.MaxSpeed;

        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();
        
        _mainCamera = Camera.main;
    }

    private void Update()
    {

        //Adjust player maximum horizontal speed 
        if (_noSpeedCapTimeLeft > 0)
        {
            float percentage = ( _noSpeedCapTimeLeft / NoSpeedCapTime) * 10;
            _noSpeedCapTimeLeft -= Time.deltaTime;
            _playerGroundController.Value.MaxSpeed = (_playerMaxSpeed * percentage) + _playerMaxSpeed;
        }
        else
        {
            _playerGroundController.Value.MaxSpeed = _playerMaxSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         * Sound
         */

        if (Commons.IsVectorOnScreen(collision.contacts.First().point, _mainCamera))
        {
            //Play sound at normal volume if collision is player
            if (collision.gameObject.IsPlayer())
            {
                _multiSoundPlayer.PlaySound();
            }
            //Play sound at 40% volume
            else if (!collision.gameObject.GetComponent<Projectile>())
            {
                _multiSoundPlayer.PlaySound(0,1,0.4f);
            }
        }

        /*
         * Collision logic
         */
        
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 relativeVelocity = collision.relativeVelocity;
        
        //If the collsision is from the side
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
        //if the collsision is from top or bottom
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
            
            _jumpController.Value.CaptureSuccessfulJumpSnapshot();
        }
        
    }
}
