﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LinearProjectile : Projectile
{
    private Rigidbody2D _rigidbody;

    public override bool DestroyOnHitboxContact => true;
    public override bool DestroyOnGroundImpact => true;

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();
        //sets the speed in the facing direction
        _rigidbody.velocity = transform.up * Speed;
    }
}
