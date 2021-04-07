using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TimedLinearProjectile : Projectile
{
    [Tooltip("For how long (in seconds) the projectile stays before self destructing")]
    public float LivingTime;
    
    private Rigidbody2D _rigidbody;

    private float _time;

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

        _time += Time.deltaTime;
        
        //If the gameobject has lived out its life: destroy bullet
        if (_time >= LivingTime) {
            Destroy(gameObject);
        }

    }
}