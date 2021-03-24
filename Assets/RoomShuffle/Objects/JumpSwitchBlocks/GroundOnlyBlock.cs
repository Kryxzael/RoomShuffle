using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// An object that is only solid when the player is on the ground
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class GroundOnlyBlock : JumpSwitchBlock
{
    private bool _lastGroundState;
    private static RenewableLazy<JumpController> _playerJump = new RenewableLazy<JumpController>(() => FindObjectOfType<JumpController>());

    protected virtual void FixedUpdate()
    {
        if (_playerJump.Value != null)
        {
            bool currentGroundState = _playerJump.Value.OnGround2D();

            if (currentGroundState != _lastGroundState)
            {
                _lastGroundState = currentGroundState;
                Collider.enabled = currentGroundState ^ !InitiallyOn;

                SetSprite();
            }
        }
    }

    public override void Switch()
    {
        //This block does not need manual switching
    }
}
