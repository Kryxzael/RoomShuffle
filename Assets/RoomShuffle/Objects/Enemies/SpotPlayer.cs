using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyPlayerRelationship
{
    outOfSight,
    inSight,
    inDistance,
    chasing,
    puzzled
}

public class SpotPlayer : MonoBehaviour
{
    
    public float reactionTime;
    public float spottingRadius;
    public float spottingRadiusChasingScale;

    public EnemyPlayerRelationship epr { get; private set; }
    private float reactionTimeLeft;
    private GameObject _player;
    private Collider2D _collider;
    void Start()
    {
        _player = CommonExtensions.GetPlayer();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        Debug.Log(reactionTimeLeft);

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
        
        if (totalDistance < spottingRadius || 
            epr == EnemyPlayerRelationship.chasing && totalDistance < spottingRadius * spottingRadiusChasingScale)
        {
            epr = EnemyPlayerRelationship.inDistance;
            GetComponent<SpriteRenderer>().color = Color.blue;
            
            if (Physics2D.Raycast(_collider.bounds.center, new Vector2(distanceX, distanceY), totalDistance).collider.gameObject == _player)
            {

                GetComponent<SpriteRenderer>().color = Color.yellow;
                epr = EnemyPlayerRelationship.inSight;

                reactionTimeLeft -= Time.deltaTime;

                if (reactionTimeLeft <= 0)
                {
                    GetComponent<SpriteRenderer>().color = Color.red;
                    epr = EnemyPlayerRelationship.chasing;
                }
            }
            else
            {
                if (reactionTimeLeft <= 0)
                {
                    reactionTimeLeft = reactionTime;
                }
                reactionTimeLeft += Time.deltaTime;
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
                GetComponent<SpriteRenderer>().color = Color.green;
                epr = EnemyPlayerRelationship.outOfSight;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.magenta;
                epr = EnemyPlayerRelationship.puzzled;
            }

        }
    }
}
