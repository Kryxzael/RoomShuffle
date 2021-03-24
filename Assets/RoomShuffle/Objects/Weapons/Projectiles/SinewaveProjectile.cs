using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SinewaveProjectile : Projectile
{
    [Tooltip("The speed of the amplitude of the wave")]
    public float WaveSpeed = 10f;
    
    private Vector3 linearPosition;
    private float time;
    
    protected override void Start()
    {
        base.Start();
        
        linearPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        
        time += Time.deltaTime;
        
        //Update position in firing direction
        transform.position = linearPosition;
        transform.position += transform.up * (Speed * Time.deltaTime);
        linearPosition = transform.position;

        //Update position in wavy direction
        int invert = (transform.up.x > 0) ? -1 : 1;
        transform.position += transform.right * ((float)Math.Sin(time * WaveSpeed) * 0.75f * invert);


    }
}