using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

/// <summary>
/// A weapon that shoots bullets in a circle around the player
/// </summary>
[CreateAssetMenu(menuName = "Weapons/Circle Shot")]
public class CircleShotWeapon : WeaponTemplate
{
    [Tooltip("The ammunition to use when firing")]
    public Projectile Ammunition;

    [Tooltip("How many bullets will be fired in one blast")]
    public int BlastCount;

    protected override void OnFire(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {
        for (int i = 0; i < BlastCount; i++)
        {
            Projectile newAmmo = Instantiate(
                original: Ammunition,
                position: shooter.GetProjectilesSpawnPoint(),
                rotation: Quaternion.identity
            );

            newAmmo.Direction = direction;
            newAmmo.transform.Rotate(i * (360f/BlastCount));

            WeaponFireHurtbox hurtbox = newAmmo.GetComponent<WeaponFireHurtbox>();
            hurtbox.Shooter = shooter;
            hurtbox.Weapon = instance;
        }
    }
}
