using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Describes the relationship between a spotter
/// </summary>
public enum SpotterPlayerRelationship
{
    /// <summary>
    /// The player is outside the spotter's vision radius
    /// </summary>
    OutOfRadius,

    /// <summary>
    /// The spotter has spotted the player, but has not yet started chasing them
    /// </summary>
    Spotted,

    /// <summary>
    /// The player is within the spotter's vision radius, but cannot be seen by the spotter
    /// </summary>
    HiddenInRadius,

    /// <summary>
    /// The spotter is chasing down the player
    /// </summary>
    Chasing,
    
    /// <summary>
    /// The spotter is trying to chase the player when the player is out of radius
    /// </summary>
    BlindChasing,

    /// <summary>
    /// The spotter has seen the player but has not lost them before it could enter chasing mode
    /// </summary>
    Puzzled
}

/// <summary>
/// State machine that allows an object to spot the player
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SpotPlayer : MonoBehaviour
{
    [Tooltip("The amount of seconds the state-machine will use to go from spotted to chasing mode.")]
    public float ReactionTime;
    
    [Tooltip("The amount of seconds the spotter will BLINDLY chase the player when out of sight.")]
    public float BlindChaseTime;

    [Tooltip("The radius around the object where the player can be spotted.")]
    public float SpottingRadius;

    [Tooltip("The scalar that will be applied to the spotting radius when the spotter is in chase mode.")]
    public float SpottingRadiusChasingScale;
    
    [Tooltip("Chooses if the enemy can spot the player trough other enemies")]
    public bool EnemiescanSeeTroughEnemies;

    /// <summary>
    /// The current state of the spotter
    /// </summary>
    public SpotterPlayerRelationship State { get; private set; }

    //How much time there is left before the spotter goes from spotted mode to chasing mode or how long it will be puzzled for
    private float ReactionTimeLeft;
    
    //How much time there is left before the spotter stops chasing blindly the player after loosing sight of the player
    private float BlindChaseTimeLeft;
    
    //The Direction the enemy will blindly chase the player in.
    public Vector2 BlindChaseDirection { get; private set; }

    /* *** */

    private GameObject _player;
    private Collider2D _collider;

    void Awake()
    {
        _player = CommonExtensions.GetPlayer();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(_player.transform.position, transform.position);   
        ReactionTimeLeft = Mathf.Clamp(ReactionTimeLeft, 0, ReactionTime);
        
        //The player is in the enemy's spotting distance (or chasing spotting distance)
        if (distanceToPlayer < SpottingRadius || 
            (State == SpotterPlayerRelationship.Chasing && distanceToPlayer < SpottingRadius * SpottingRadiusChasingScale))
        {

            if (BlindChaseTimeLeft > 0)
            {
                State = SpotterPlayerRelationship.BlindChasing;
                BlindChaseTimeLeft -= Time.deltaTime;
            }
            else
            {
                State = SpotterPlayerRelationship.HiddenInRadius;
            }

            //Checks vision cone of enemy. If a raycast towards the player hits it, the player is spotted. If not, something's in the way

            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(_collider.bounds.center, _player.transform.position - transform.position, distanceToPlayer);

            bool inSight = !hits.Any(x => (x.transform.gameObject.tag != "Enemy" || !EnemiescanSeeTroughEnemies) && x.transform.gameObject.tag != "Player");

            if (inSight)
            {
                State = SpotterPlayerRelationship.Spotted;

                ReactionTimeLeft -= Time.deltaTime;

                //The enemy has spotted the player for long enough and is now chasing
                if (ReactionTimeLeft <= 0)
                {
                    State = SpotterPlayerRelationship.Chasing;
                    
                    //resets the blind chasing counter
                    BlindChaseTimeLeft = BlindChaseTime;
                    
                    //update the direction the enemy will blindly chase in.
                    BlindChaseDirection = (_player.transform.position - transform.position).normalized;
                }
            }

            //Something's in the way
            else
            {
                ReactionTimeLeft += Time.deltaTime;
            }
        }

        //The player is not in the spotting distance
        else
        {
            // The enemy is blindly chasing the player
            if (BlindChaseTimeLeft > 0)
            {
                State = SpotterPlayerRelationship.BlindChasing;
                BlindChaseTimeLeft -= Time.deltaTime;
            }
            // the enemy is done blindly chasing the player
            else
            {
                if (ReactionTimeLeft <= 0)
                {
                    ReactionTimeLeft = 0;
                }
                ReactionTimeLeft += Time.deltaTime;
                
                if (ReactionTimeLeft >= ReactionTime)
                {
                    State = SpotterPlayerRelationship.OutOfRadius;
                }
                else
                {
                    State = SpotterPlayerRelationship.Puzzled;
                }
            }
            
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (_collider == null)
            return;

        switch (State)
        {
            case SpotterPlayerRelationship.OutOfRadius:
                Gizmos.color = default;
                break;
            case SpotterPlayerRelationship.Spotted:
                Gizmos.color = Color.yellow;
                break;
            case SpotterPlayerRelationship.HiddenInRadius:
                Gizmos.color = Color.white;
                break;
            case SpotterPlayerRelationship.Chasing:
                Gizmos.color = Color.red;
                break;
            case SpotterPlayerRelationship.Puzzled:
                Gizmos.color = Color.magenta;
                break;
            default:
                break;
        }

        Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size * 1.1f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_collider.bounds.center, SpottingRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_collider.bounds.center, SpottingRadius * SpottingRadiusChasingScale);
    }
}