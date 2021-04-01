using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoomerangProjectile : Projectile
{
    [Tooltip("The force the boomerang will be pushed back with when thrown")]
    public float Pushbackforce = 1000f;
    
    [Tooltip("The force the boomerang will be pushed back with when thrown")]
    public float SpeedCap = 20f;
    
    private Rigidbody2D _rigidbody;

    public override bool DestroyOnGroundImpact => true;
    public override bool DestroyOnHitboxContact => false;

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.up * Speed;
    }

    protected override void Update()
    {
        base.Update();

        //Caps the speed to SpeedCap
        if (_rigidbody.velocity.magnitude > SpeedCap)
            return;
        
        //add force to the bullet in the opposite direction of the throw
        _rigidbody.AddForce(-transform.up * (Time.deltaTime * Pushbackforce));
    }
}