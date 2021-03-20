using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Allows an object to be freely moved around with WASD without collision or physics
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class NoclipMotion : MonoBehaviour
{
    //The state of the noclip cheat last frame (we don't want to change physics mode every frame)
    private bool _lastKnownNoclipState;

    /* *** */

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    /* *** */

    [Tooltip("The speed (in units per frame) the player will move when nocliping. This is NOT in velocity")]
    public float NoclipSpeed = 0.005f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        /*
         * Update noclip mode
         */
        if (_lastKnownNoclipState != Cheats.Noclip)
        {
            //Enabled noclip
            if (Cheats.Noclip)
            {
                _rigidbody.bodyType = RigidbodyType2D.Static;
                _collider.enabled = false;
            }

            //Disable noclip
            else
            {
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                _collider.enabled = true;
            }

            _lastKnownNoclipState = Cheats.Noclip;
        }

        /*
         * If noclip is enabled, allow motion
         */
        if (Cheats.Noclip)
        {
            Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            transform.Translate(inputVector * NoclipSpeed);
        }
    }
}
