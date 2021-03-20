using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(WeaponFireHurtbox))]
public abstract class Projectile : MonoBehaviour
{
    /// <summary>
    /// Gets or sets the direction the projectile will travel in
    /// </summary>
    public Vector2 Direction
    {
        get
        {
            return transform.up;
        }
        set
        {
            transform.up = value;
        }
    }

    public float Speed;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
