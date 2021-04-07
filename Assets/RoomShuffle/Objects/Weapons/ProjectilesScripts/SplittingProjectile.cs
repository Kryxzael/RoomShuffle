using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SplittingProjectile : Projectile
{
    private static bool _isQuitting;

    [Tooltip("The projectile that will be fired when the original projectile 'explodes'")]
    public Projectile SplitterProjectile;

    [Tooltip("The number of bullets that spreads after 'exploding'")]
    public int BlastCount = 20;

    [Tooltip("The Range of the splitting projectile")]
    public float SplittingProjectileRange;
    
    [Tooltip("The Range of the splitting projectile")]
    public int SplittingProjectileDamage;

    [Tooltip("The time in secounds between each split")]
    public float TimeBetweenEachSplit;
    
    private Rigidbody2D _rigidbody;
    private WeaponFireHurtbox _weaponFireHurtBox;
    private float _originalRange;
    private WeaponInstance SplittingProjectileWeaponInstance;
    private int numberOfSplits = 0;
    private float _time;

    public override bool DestroyOnHitboxContact => true;
    public override bool DestroyOnGroundImpact => true;

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
        _weaponFireHurtBox = GetComponentInChildren<WeaponFireHurtbox>();

        //Create a weaponInstance for splittingprojectile
        SplittingProjectileWeaponInstance = GetComponentInChildren<WeaponFireHurtbox>().Weapon.Template.CreateWeaponInstance();
        SplittingProjectileWeaponInstance.Range = SplittingProjectileRange;
        SplittingProjectileWeaponInstance.BaseDamage = SplittingProjectileDamage;
    }
    
    protected override void Update()
    {
        base.Update();

        //add linear speed
        _rigidbody.velocity = transform.up * Speed;

        //The projectile shall only split on destroy
        if (TimeBetweenEachSplit <= 0)
            return;

        if ((int)(_time / TimeBetweenEachSplit) > numberOfSplits)
        {
            numberOfSplits++;
            Split();
        }

        _time += Time.deltaTime;
    }

    //Spawn a bunch of projectiles
    private void Split()
    {
        for (int i = 0; i < BlastCount; i++)
        {
            //create bullet
            Projectile newAmmo = Instantiate(
                original: SplitterProjectile,
                position: transform.position,
                rotation: Quaternion.identity
            );

            //rotate bullet
            newAmmo.Direction = transform.up;
            newAmmo.transform.Rotate(i * (360f/BlastCount));

            //add hurtbox
            WeaponFireHurtbox hurtbox = newAmmo.GetComponentInChildren<WeaponFireHurtbox>();
            hurtbox.Shooter = _weaponFireHurtBox.Shooter;
            hurtbox.Weapon = SplittingProjectileWeaponInstance;
        }
    }

    private void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    private void OnDestroy()
    {
        //Do not split if the reason the projectile is being destroyed is due to application quit
        //Unity doesn't like that
        if (_isQuitting)
            return;

        Split();
    }
}