using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SplittingProjectile : Projectile
{
    [Tooltip("The projectile that will be fired when the original projectile 'explodes'")]
    public Projectile SplitterProjectile;
    
    [Tooltip("The time it takes before the bullet explodes")]
    public float timeBeforeSplitting = 1f;
    
    [Tooltip("The number of bullets that speads after 'exploding'")]
    public int blastCount = 20;
    
    private Rigidbody2D _rigidbody;
    private float _time = 0;
    private bool _doneSplitting = false;
    private WeaponFireHurtbox _weaponFireHurtBox;
    

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
        _weaponFireHurtBox = GetComponent<WeaponFireHurtbox>();
    }
    
    protected override void Update()
    {
        base.Update();
        
        //add time to timer
        _time += Time.deltaTime;
        
        //add linear speed
        _rigidbody.velocity = transform.up * Speed;
        
        //If the timer hasn't reached its goal or the splitting has already happened: return
        if (_time < timeBeforeSplitting || _doneSplitting) //TODO add explode on wall function
            return;
        
        //for each bullet in total number of exploding bullets
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
            WeaponFireHurtbox hurtbox = newAmmo.GetComponent<WeaponFireHurtbox>();
            hurtbox.Shooter = _weaponFireHurtBox.Shooter;
            hurtbox.Weapon = _weaponFireHurtBox.Weapon;
        }
        
        _doneSplitting = true;
    }
}