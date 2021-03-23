using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A weapon that shoots mulitple bullets in burst linearly
/// </summary>
[CreateAssetMenu(menuName = "Weapons/Automatic Shot")]
public class AutomaticShotWeapon : WeaponTemplate
{
    [Tooltip("The ammunition to use when firing")]
    public Projectile Ammunition;

    [Tooltip("How many bullets will be fired in one blast")]
    public int ClusterCount;
    
    [Tooltip("Tiem between each shot")]
    public float WaitTime = 0.05f;

    protected override void OnFire(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {
        shooter.StartCoroutine(CoShootBullets(instance, shooter));
    }

    /// <summary>
    /// Shoots bullets with identical time between each bullet
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="shooter"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator CoShootBullets(WeaponInstance instance, WeaponShooterBase shooter)
    {
        for (int i = 0; i < ClusterCount; i++)
        {
            Projectile newAmmo = Instantiate(
                original: Ammunition,
                position: shooter.GetProjectilesSpawnPoint(),
                rotation: Quaternion.identity
            );

            newAmmo.Direction = shooter.GetCurrentAimingDirection();

            WeaponFireHurtbox hurtbox = newAmmo.GetComponent<WeaponFireHurtbox>();
            hurtbox.Shooter = shooter;
            hurtbox.Weapon = instance;
            
            //Waiting for next bullet
            yield return new WaitForSeconds(WaitTime);
        }
    }


}