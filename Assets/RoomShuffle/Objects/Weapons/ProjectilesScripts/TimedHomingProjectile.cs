using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TimedHomingProjectile : Projectile
{
    [Tooltip("The force the bullet will be pushed with.")]
    public float DragForce = 300;

    [Tooltip("Maximum speed for the bullet")]
    public float MaxSpeed = 20;

    [Tooltip("The time the projectile will be homing before they self destruct")]
    public float LivingTime = 5;

    private Rigidbody2D _rigidbody;
    private GameObject[] _enemyList;
    private GameObject _nearestEnemy;
    private float _timeLived;
    private bool _finalDirectionChosen = false;

    public override bool DestroyOnHitboxContact => false;
    public override bool DestroyOnGroundImpact => false;

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.up * Speed;
        _enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        
        _nearestEnemy = null;
        
        //If there are no enemies: return
        if (_enemyList == null || !_enemyList.Any())
            return;
        
        _nearestEnemy = _enemyList[0];
        
        //Sets the nearestEnemy variable to the nearest enemy.
        float smallestDistance = Vector3.Distance(transform.position, _enemyList[0].transform.position);
        
        foreach (GameObject enemy in _enemyList)
        {
            float newDistance = Vector3.Distance(transform.position, enemy.transform.position);
            
            if (newDistance < smallestDistance)
            {
                smallestDistance = newDistance;
                _nearestEnemy = enemy;
            }
        }
    }

    protected override void Update()
    {
        
        base.Update();

        //if the bullet has lived for long enough: Self destruct
        _timeLived += Time.deltaTime;
        if (_timeLived >= LivingTime)
        {
            Destroy(gameObject);
        }

        //If there is no enemies: go in random direction
        if (!_nearestEnemy)
        {
            if (!_finalDirectionChosen)
            {
                transform.up = Random.insideUnitCircle.normalized;
                _rigidbody.velocity = transform.up * Speed;
                _finalDirectionChosen = true;
                return;
            }
            else
            {
                return;
            }

        }
        
        //Pushes the bullet towards the enemy.
        _rigidbody.AddForce((_nearestEnemy.transform.position - transform.position).normalized * (Speed * Time.deltaTime * DragForce));

        //Caps the speed
        if (_rigidbody.velocity.magnitude > MaxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * MaxSpeed;
        }

    }
}