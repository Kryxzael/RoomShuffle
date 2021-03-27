using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using UnityRandom = UnityEngine.Random;

public class EnemySpawnPoint : MonoBehaviour
{
    [Tooltip("The set to spawn enemies from")]
    public EnemySet SpawnSet;

    [Tooltip("The radius around the spawn point the enemy can be spawned at")]
    public float SpawningRadius = 1f;

    [Header("Spawn Selection")]
    [Tooltip("Will the spawned enemy be airborne")]
    public bool SpawnAirborne;

    [Tooltip("Will the spawned be a secondary enemy")]
    public bool SpawnSecondaryEnemy;

    private void Start()
    {
        SpawnEnemy();
    }

    /// <summary>
    /// Spawns an enemy at or around the spawn point
    /// </summary>
    public GameObject SpawnEnemy()
    {
        /*
         * Pick spawn location
         */
        Vector2 spawnPosition = transform.position;

        //Airborne enemies spawn in a circle around the spawn point
        if (SpawnAirborne)
            spawnPosition += (Vector2)UnityRandom.insideUnitSphere * SpawningRadius;

        //Grounded enemies spawn with a potential horizontal offset of the spawn point
        else
            spawnPosition += Vector2.right * UnityRandom.Range(-SpawningRadius, SpawningRadius);


        /*
         * Instantiate the enemy
         */

        //Chose enemy to spawn
        GameObject prefab;

        if (SpawnAirborne)
        {
            if (SpawnSecondaryEnemy)
                prefab = SpawnSet.SecondaryAir;

            else
                prefab = SpawnSet.PrimaryAir;
        }
        else
        {
            if (SpawnSecondaryEnemy)
                prefab = SpawnSet.SecondaryGround;

            else
                prefab = SpawnSet.PrimaryGround;
        }

        return Commons.InstantiateInCurrentLevel(prefab, spawnPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Renders airborne spawning radius (circle)
        if (SpawnAirborne)
        {
            Gizmos.DrawWireSphere(transform.position, SpawningRadius);
        }

        //Renders ground spawning radius (cube)
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(SpawningRadius * 2f, 1f, 1f));
            
        }
    }
}
