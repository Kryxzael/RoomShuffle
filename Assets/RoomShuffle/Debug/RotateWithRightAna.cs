using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Rotates the object with the right analog stick
/// </summary>
public class RotateWithRightAna : MonoBehaviour
{
    [Tooltip("The amount of degrees to move rotate per frame")]
    public float Speed = 1f;

    private void Update()
    {
        transform.Rotate(Input.GetAxisRaw("HorizontalAlt") * Speed);
    }
}