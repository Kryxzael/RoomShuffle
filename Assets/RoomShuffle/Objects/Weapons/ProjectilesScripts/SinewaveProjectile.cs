using System;
using UnityEngine;

public class SinewaveProjectile : Projectile
{
    [Tooltip("The speed of the amplitude of the wave")]
    public float WaveSpeed = 10f;
    
    private Vector3 _linearPosition;
    private float _time;

    public override bool DestroyOnHitboxContact => false;
    public override bool DestroyOnGroundImpact => false;

    protected override void Start()
    {
        base.Start();
        
        _linearPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        
        _time += Time.deltaTime;
        
        //Update position in firing direction
        transform.position = _linearPosition;
        transform.position += transform.up * (Speed * Time.deltaTime);
        _linearPosition = transform.position;

        //Update position in wavy direction
        int invert = (transform.up.x > 0) ? -1 : 1;
        transform.position += transform.right * ((float)Math.Sin(_time * WaveSpeed) * 0.75f * invert);


    }
}