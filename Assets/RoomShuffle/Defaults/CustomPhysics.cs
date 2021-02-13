using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global physics that should be applied to all rigidbodies that intend to move
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class CustomPhysics : MonoBehaviour
{
    private Rigidbody2D _rigid;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Stick to ground when on slope
        if (this.OnGround2D())
        {
            if (_rigid.velocity.x != 0f)
            {
                _rigid.gravityScale = 0f;
            }
            else
            {
                _rigid.gravityScale = 0f;
                _rigid.SetVelocityY(currentY => Mathf.Max(currentY, 0f));
            }
        }
        else
        {
            _rigid.gravityScale = 1f;
        }
    }
}
