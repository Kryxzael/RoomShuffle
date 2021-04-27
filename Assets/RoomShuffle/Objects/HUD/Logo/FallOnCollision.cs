using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables an object's rigidbody component when it receives collision
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class FallOnCollision : MonoBehaviour
{
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigid.bodyType = RigidbodyType2D.Dynamic;
    }
}
