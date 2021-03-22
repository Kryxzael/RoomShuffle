using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Vector2 = System.Numerics.Vector2;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingProjectile : Projectile
{
    private Rigidbody2D _rigidbody;
    private GameObject[] _enemyList;
    private GameObject _nearestEnemy;

    protected void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.up * Speed;
        _enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        
        _nearestEnemy = null;
        
        //If there are no enemies: return
        if (_enemyList == null)
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

    private void Update()
    {
        //The force the bullet will be pushed with.
        const float DRAGFORCE = 300;
        
        //Maximum speed for the bullet
        const float MAXSPEED = 20;

        //If there is no enemies: act as a linear bullet
        if (!_nearestEnemy)
        {
            _rigidbody.velocity = transform.up * Speed;
            return;
        }
        
        //Pushes the bullet towards the enemy.
        _rigidbody.AddForce((_nearestEnemy.transform.position - transform.position).normalized * (Speed * Time.deltaTime * DRAGFORCE));

        //Caps the speed
        if (_rigidbody.velocity.magnitude > MAXSPEED)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * MAXSPEED;
        }

    }
}