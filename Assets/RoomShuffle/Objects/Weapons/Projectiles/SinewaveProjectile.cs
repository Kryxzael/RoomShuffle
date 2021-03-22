using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SinewaveProjectile : Projectile
{
    private Vector3 linearPosition;
    private float time;
    
    protected void Awake()
    {
        linearPosition = transform.position;
        
    }

    private void Update()
    {
        const float SPEED = 10f;
        time += Time.deltaTime;

        //Update position in firing direction
        transform.position = linearPosition;
        transform.position += transform.up * SPEED * Time.deltaTime;
        linearPosition = transform.position;

        //Update position in wavy direction
        int invert = (transform.up.x > 0) ? -1 : 1;
        transform.position += transform.right * (float)Math.Sin(time * 10f) * 0.75f * invert;


    }
}