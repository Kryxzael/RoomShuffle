using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SplittingProjectile : Projectile
{
    [Tooltip("The projectile that will be fired when the original projectile 'explodes'")]
    public Projectile SplitterProjectile;

    [Tooltip("The number of bullets that speads after 'exploding'")]
    public int blastCount = 20;
    
    private Rigidbody2D _rigidbody;
    private bool _doneSplitting = false;
    private WeaponFireHurtbox _weaponFireHurtBox;
    

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
        _weaponFireHurtBox = GetComponentInChildren<WeaponFireHurtbox>();
    }
    
    protected override void Update()
    {
        base.Update();

        //add linear speed
        _rigidbody.velocity = transform.up * Speed;
        
    }

    //When projectile gets destroyed: Spawn a bunch of projectiles
    private void OnDestroy()
    {
        for (int i = 0; i < blastCount; i++)
        {
            //create bullet
            Projectile newAmmo = Instantiate(
                original: SplitterProjectile,
                position: transform.position,
                rotation: Quaternion.identity
            );

            //rotate bullet
            newAmmo.Direction = transform.up;
            newAmmo.transform.Rotate(i * (360f/blastCount));

            //add hurtbox
            WeaponFireHurtbox hurtbox = newAmmo.GetComponentInChildren<WeaponFireHurtbox>();
            hurtbox.Shooter = _weaponFireHurtBox.Shooter;
            hurtbox.Weapon = _weaponFireHurtBox.Weapon;
        }
    }
}