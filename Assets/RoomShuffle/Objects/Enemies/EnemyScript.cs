using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Base class for scripts that can easily access enemy information
/// </summary>
public abstract class EnemyScript : MonoBehaviour
{
    private readonly RenewableLazy<EnemyBase> _enemy;

    /// <summary>
    /// Gets the enemy base script of this enemy
    /// </summary>
    public EnemyBase Enemy
    {
        get
        {
            if (_enemy.Value == null)
                throw new InvalidOperationException("This script can only be used on objects that has a parent EnemyBase object");

            return _enemy.Value;
        }
    }

    //Yes I'm using constructors. Unity can't stop me (I hope)
    public EnemyScript()
    {
        _enemy = new RenewableLazy<EnemyBase>(() => GetComponentInParent<EnemyBase>());
    }
}
