using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpotterMovementPattern : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float spottingRadius;
    public float reactionTime;
    public float minWaitTime;
    public float maxWaitTime;
    
    private Rigidbody2D _rigid;
    private Collider2D _collider;
    private GameObject _player;
    private Collider2D _playerCollider;
    private float reactionTimeLeft;
    private float waitTime;
    private bool walk = true;
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _player = CommonExtensions.GetPlayer();
        _playerCollider = _player.GetComponent<Collider2D>();
        
        int direction = Random.Range(0,2) == 1 ? 1 : -1;
        _rigid.velocity = new Vector2(direction * walkSpeed, _rigid.velocity.y );
        
    }
    
    void Update()
    {

        float playerX = _player.transform.position.x;
        float playerY = _player.transform.position.y;
        float enemyX = transform.position.x;
        float enemyY = transform.position.y;

        float distanceX = playerX - enemyX;
        float distanceY = playerY - enemyY;
        float positiveDistanceX = Math.Abs(distanceX);
        float positiveDistanceY = Math.Abs(distanceY);

        float totalDistance = (float)Math.Sqrt(positiveDistanceX*positiveDistanceX + positiveDistanceY*positiveDistanceY);

        reactionTimeLeft = Mathf.Clamp(reactionTimeLeft, 0, reactionTime);
        
        GetComponent<SpriteRenderer>().color = Color.green;
        if ((totalDistance < spottingRadius &&
            Physics2D.Raycast(_collider.bounds.center, new Vector2(distanceX, distanceY), totalDistance).collider.gameObject == _player))
        { //Player is spotted
            
            reactionTimeLeft -= Time.deltaTime;
            _rigid.velocity = new Vector2(0,_rigid.velocity.y);
            
            GetComponent<SpriteRenderer>().color = Color.yellow;

            if (reactionTimeLeft <= 0)
            { //Player is being chased
                GetComponent<SpriteRenderer>().color = Color.red;
                _rigid.velocity = new Vector2(runSpeed * Math.Sign(playerX - enemyX), _rigid.velocity.y);
            }
        }
        else
        {
            if (reactionTimeLeft <= 0)
            {
                reactionTimeLeft = reactionTime;
            }

            reactionTimeLeft += Time.deltaTime;

            if (reactionTimeLeft >= reactionTime)
            {

                if (walk)
                { //Walking

                    if (_rigid.velocity.x == 0)
                    {
                        int direction = Math.Sign(Random.value - 0.5);
                        _rigid.velocity = new Vector2(direction * walkSpeed, _rigid.velocity.y);
                    }

                    if (Physics2D.Raycast(_collider.bounds.center, Vector2.left, _collider.bounds.extents.x + 0.02f) ||
                        Physics2D.Raycast(new Vector2(_collider.bounds.center.x, _collider.bounds.center.y + _collider.bounds.extents.y), Vector2.left, _collider.bounds.extents.x + 0.02f) ||
                        !Physics2D.Raycast(new Vector2(_collider.bounds.center.x - _collider.bounds.extents.x, _collider.bounds.center.y), Vector2.down, _collider.bounds.extents.y + 1.5f))
                    {
                        _rigid.velocity = _rigid.velocity = new Vector2(walkSpeed, _rigid.velocity.y);
                    }
                    else if (Physics2D.Raycast(_collider.bounds.center, Vector2.right, _collider.bounds.extents.x + 0.02f) ||
                             Physics2D.Raycast(new Vector2(_collider.bounds.center.x, _collider.bounds.center.y + _collider.bounds.extents.y), Vector2.right, _collider.bounds.extents.x + 0.02f) ||
                             !Physics2D.Raycast(new Vector2(_collider.bounds.center.x + _collider.bounds.extents.x, _collider.bounds.center.y), Vector2.down, _collider.bounds.extents.y + 1.5f))
                    {
                        _rigid.velocity = _rigid.velocity = new Vector2(-walkSpeed, _rigid.velocity.y);
                    }
                    else
                    {
                        _rigid.velocity = _rigid.velocity =
                            new Vector2(Math.Sign(_rigid.velocity.x) * walkSpeed, _rigid.velocity.y);
                    }

                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0)
                    {
                        walk = !walk;
                        waitTime = Random.Range(minWaitTime, maxWaitTime);
                    }
                }
                else
                { //Stationary
                    _rigid.velocity = new Vector2(0,_rigid.velocity.y);
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0)
                    {
                        walk = !walk;
                        waitTime = Random.Range(minWaitTime, maxWaitTime);
                    }
                }
            }
        }
    }
}