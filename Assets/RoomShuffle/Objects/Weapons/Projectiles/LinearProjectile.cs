using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LinearProjectile : Projectile
{
    private Rigidbody2D _rigidbody;

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //sets the speed in the facing direction
        _rigidbody.velocity = transform.up * Speed;
    }
}
