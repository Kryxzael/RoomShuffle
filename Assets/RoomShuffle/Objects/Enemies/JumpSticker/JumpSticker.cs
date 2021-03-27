using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityRandom = UnityEngine.Random;

/// <summary>
/// An enemy that jumps between walls
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class JumpSticker : MonoBehaviour
{
    [Tooltip("How big of a radius the enemy should look for walls in")]
    public float SearchRadius = 5f;

    [Header("Speeds")]
    [Tooltip("The time between each jump")]
    public RandomValueBetween WaitTime = new RandomValueBetween(1f, 5f);

    [Tooltip("The force (speed) the enemy will jump at")]
    public float JumpForce;

    /* *** */
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ScheduleJump();
    }

    /// <summary>
    /// Makes the enemy attempt to find a wall and jump to it
    /// </summary>
    private bool TryJump()
    {
        Debug.Log("I'm walkin' here");

        /*
         * Try to find a wall to jump to
         */
        const int FIND_WALL_MAX_ATTEMPTS = 10;
        RaycastHit2D foundWallHit = default;
        Vector2 jumpDirection = default;

        for (int i = 0; i < FIND_WALL_MAX_ATTEMPTS; i++)
        {
            //Choose a random direction
            jumpDirection = UnityRandom.onUnitSphere;

            //Perform the cast
            foundWallHit = Physics2D.Raycast(transform.position, jumpDirection, SearchRadius);

            //If the wall was found, break the loop and stop the attempts
            if (foundWallHit)
                break;
        }

        //No wall was found. Give up
        if (!foundWallHit)
        {
            Debug.Log("Nevermind. I'm not going anywhere :(");
            ScheduleJump();
            return false;
        }

        /*
         * Jump to said wall
         */
        _rigidbody.velocity = jumpDirection * Commons.GetEffectValue(JumpForce, EffectValueType.EnemySpeed);

        //Schedule the next jump
        ScheduleJump();

        return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Stop the enemy when it hits something
        _rigidbody.velocity = default;
    }

    /// <summary>
    /// Schedules the next jump randomly
    /// </summary>
    private void ScheduleJump() 
    {
        Invoke(nameof(TryJump), Commons.GetEffectValue(WaitTime.Pick(), EffectValueType.EnemyWaitTime));
    }
}
