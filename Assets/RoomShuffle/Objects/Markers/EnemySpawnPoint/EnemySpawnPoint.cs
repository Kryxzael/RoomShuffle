using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using UnityRandom = UnityEngine.Random;

public class EnemySpawnPoint : MonoBehaviour
{
    [Tooltip("The radius around the spawn point the enemy can be spawned at")]
    public float SpawningRadius = 1f;

    [Header("Spawn Selection")]
    [Tooltip("Will the spawned enemy be airborne")]
    public bool SpawnAirborne;

    /// <summary>
    /// Spawns an enemy at or around the spawn point
    /// </summary>
    public EnemyBase SpawnEnemy(System.Random random, IList<RoomParameters.EnemyWithSpawnRate> groundEnemies, IList<RoomParameters.EnemyWithSpawnRate> airEnemies)
    {
        /*
         * Pick spawn location
         */
        var spawnPosition = transform.Position2D();

        //Airborne enemies spawn in a circle around the spawn point
        if (SpawnAirborne)
            spawnPosition += (Vector2)UnityRandom.insideUnitSphere * SpawningRadius;

        //Grounded enemies spawn with a potential horizontal offset of the spawn point
        else
            spawnPosition += Vector2.right * UnityRandom.Range(-SpawningRadius, SpawningRadius);


        /*
         * Instantiate the enemy
         */

        RoomParameters.EnemyWithSpawnRate candidate;

        //Chose enemy to spawn based on settings
        do
        {
            if (SpawnAirborne)
                candidate = airEnemies[random.Next(airEnemies.Count)];

            else 
                candidate = groundEnemies[random.Next(groundEnemies.Count)];

        } while (random.Next() < candidate.SpawnRate);

        //Spawn and return the enemy
        return Commons.InstantiateInCurrentLevel(candidate.Enemy, spawnPosition);
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
