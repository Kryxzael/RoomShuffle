using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A solid block that is destroyed when all enemies in the room are defeated
/// </summary>
public class EradicationLockBlock : MonoBehaviour
{
    /// <summary>
    /// Gets how many enemies are left in the stage
    /// </summary>
    public static int EnemiesLeft
    {
        get
        {
            return FindObjectsOfType<EnemyBase>().Count();
        }
    }

    private void LateUpdate()
    {
        Commons.SoundtrackPlayer.AddAdrenalineTrigger(this, 0.5f);

        if (EnemiesLeft == 0)
            Destroy(gameObject);
    }
}
