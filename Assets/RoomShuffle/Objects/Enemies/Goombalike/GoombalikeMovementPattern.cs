using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoombalikeMovementPattern : MonoBehaviour
{
    public float walkSpeed;
    private Rigidbody2D _rigid;
    private Collider2D _collider;
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        
        int direction = Random.Range(0,2) == 1 ? 1 : -1;
        _rigid.velocity = new Vector2(direction * walkSpeed, _rigid.velocity.y );
        
    }
    
    void Update()
    {
        if (Physics2D.Raycast(_collider.bounds.center, Vector2.left, _collider.bounds.extents.x + 0.02f) ||
            Physics2D.Raycast(new Vector2(_collider.bounds.center.x,_collider.bounds.center.y + _collider.bounds.extents.y), Vector2.left, _collider.bounds.extents.x + 0.02f))
        {
            _rigid.velocity = _rigid.velocity = new Vector2(walkSpeed, _rigid.velocity.y);
        } 
        else if (Physics2D.Raycast(_collider.bounds.center, Vector2.right, _collider.bounds.extents.x + 0.02f) ||
                 Physics2D.Raycast(new Vector2(_collider.bounds.center.x,_collider.bounds.center.y + _collider.bounds.extents.y), Vector2.right, _collider.bounds.extents.x + 0.02f))
        {
            _rigid.velocity = _rigid.velocity = new Vector2(-walkSpeed, _rigid.velocity.y);
        }
        else
        {
            _rigid.velocity = _rigid.velocity = new Vector2(Math.Sign(_rigid.velocity.x) * walkSpeed, _rigid.velocity.y);
        }
    }
}
