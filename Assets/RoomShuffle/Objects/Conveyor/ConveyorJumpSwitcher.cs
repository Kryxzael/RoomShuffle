using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A controller that makes a conveyor switch its direction when the player jumps
/// </summary>
[RequireComponent(typeof(Flippable))]
public class ConveyorJumpSwitcher : JumpSwitchBase
{
    private Flippable _flippable;

    /* *** */

    [Tooltip("Is this script enabled")]
    public bool IsEnabled;

    protected void Awake()
    {
        _flippable = GetComponent<Flippable>();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override void OnJump()
    {
        if (!IsEnabled)
            return;

        //Switches the directionality of the conveyor
        _flippable.Flip();
    }
}
