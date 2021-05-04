using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Makes an object push other objects away at high speeds
/// </summary>
public class BouncyBlock : MonoBehaviour
{
    //The maximum speed the player has normally
    private float _playerMaxSpeed;

    //For how many more seconds the player's max-speed will be adjusted (after bouncing horizontally)
    private float _noSpeedCapTimeLeft = 0;

    //The main camera in the scene
    private Camera _mainCamera;

    /* *** */

    private MultiSoundPlayer _multiSoundPlayer;
    private RenewableLazy<GroundController> _playerGroundController = new RenewableLazy<GroundController>(() => CommonExtensions.GetPlayer().GetComponent<GroundController>());
    private RenewableLazy<JumpController> _playerJumpController = new RenewableLazy<JumpController>(() => CommonExtensions.GetPlayer().GetComponent<JumpController>());

    /* *** */

    [Tooltip("The minimum speed the block will push you at")]
    public float MinimumPushBackVelocity;
    
    [Tooltip("The amount your velocity will be multiplied with when holding jump")]
    public float HoldingJumpScale;

    [Tooltip("The amount of time the player will not have a speed cap")]
    public float NoSpeedCapTime;


    private void Start()
    {
        _playerMaxSpeed = _playerGroundController.Value.MaxSpeed;
        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        //Adjust player maximum horizontal speed if it is supposed to be adjusted
        if (_noSpeedCapTimeLeft > 0)
        {
            //The max-speed of the player gradually returns to normal
            float percentage = ( _noSpeedCapTimeLeft / NoSpeedCapTime) * 10;
            _noSpeedCapTimeLeft -= Time.deltaTime;
            _playerGroundController.Value.MaxSpeed = (_playerMaxSpeed * percentage) + _playerMaxSpeed;
        }
        else
        {
            //Keep the player's maximum speed normal
            _playerGroundController.Value.MaxSpeed = _playerMaxSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         * Sound
         */

        //If block is visible
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
                _multiSoundPlayer.PlaySound(volume: 0.4f);
            }
        }

        /*
         * Collision logic
         */
        
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 relativeVelocity = collision.relativeVelocity;
        
        //If the collision is from the side
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

        //if the collision is from top or bottom
        else
        {
            int direction = Math.Sign(relativeVelocity.y);
            float pushBack = Mathf.Max(Math.Abs(relativeVelocity.y), MinimumPushBackVelocity);
            
            //if the collision is with a player: check if the player is holding jump
            if (collision.gameObject == this.GetPlayer())
            {
                //If the player is holding jump: multiply the vertical speed
                if (Input.GetButton("Jump"))
                {
                    pushBack *= HoldingJumpScale;
                }

                //Tell jump controller not to allow double jumps
                _playerJumpController.Value.CaptureSuccessfulJumpSnapshot();
            }
            
            //Invert and add velocity
            collision.rigidbody.SetVelocityY(pushBack * -direction);
        }
        
    }
}
