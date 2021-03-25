using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Base class for anything that can be switched by jumping
/// </summary>
public abstract class JumpSwitchBase : MonoBehaviour
{
    /// <summary>
    /// Occurs when the player jumps
    /// </summary>
    public abstract void OnJump();
}