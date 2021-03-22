using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoomerangProjectile : Projectile
{
    private Rigidbody2D _rigidbody;

    protected void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.up * Speed;
    }

    private void Update()
    {
        if (_rigidbody.velocity.magnitude > 20f)
            return;
        
        const float PUSHBACKFORCE = 1000f; // recommended 1000
        _rigidbody.AddForce(-transform.up * (Time.deltaTime * PUSHBACKFORCE));
    }
}