using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Implements an enemy that can shoot a weapon when it sees the player
/// </summary>
[RequireComponent(typeof(EnemyWeaponShooter), typeof(SpotPlayer))]
public class ShootOnSight : MonoBehaviour
{
    /// <summary>
    /// Gets the weapon that will be fired
    /// </summary>
    public WeaponInstance WeaponInstance { get; private set; }

    /* *** */

    private EnemyWeaponShooter _shooter;
    private SpotPlayer _spotter;

    /* *** */

    [Tooltip("The weapon template that the enemy will get")]
    public WeaponTemplate Weapon;

    private void Awake()
    {
        _shooter = GetComponent<EnemyWeaponShooter>();
        _spotter = GetComponent<SpotPlayer>();
    }

    private void Start()
    {
        WeaponInstance = Weapon.CreateWeaponInstance();
    }

    private void Update()
    {
        if (Commons.PowerUpManager.HasPowerUp(PowerUp.Invincibility))
            return;

        //Shoot the player as quickly as possible when spotted
        if (_spotter.InPursuit)
        {
            if (WeaponInstance.CanFire(ignoreDurability: true))
            {
                _shooter.SetAim(_spotter.BlindChaseDirection);
                WeaponInstance.Fire(_shooter);
            }
        }
    }
}