using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

/// <summary>
/// A weapon that shoots bullets in a circle around the player
/// </summary>
[CreateAssetMenu(menuName = "Weapons/InstantHurtbox")]
public class InstantHurtbox : WeaponTemplate
{
    [Tooltip("The ammunition to use when firing")]
    public Projectile Ammunition;

    protected override void OnFire(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {
        Projectile newAmmo = Instantiate(
            original: Ammunition,
            position: shooter.GetProjectilesSpawnPoint() + direction,
            rotation: Quaternion.identity
        );

        newAmmo.Direction = direction;

        WeaponFireHurtbox hurtbox = newAmmo.GetComponentInChildren<WeaponFireHurtbox>();
        hurtbox.Shooter = shooter;
        hurtbox.Weapon = instance;

        newAmmo.transform.eulerAngles = newAmmo.transform.eulerAngles.SetY(0);

    }
}