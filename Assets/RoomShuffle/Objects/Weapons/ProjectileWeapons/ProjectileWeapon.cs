﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Projectile Weapon")]
public class ProjectileWeapon : WeaponTemplate
{
    public Projectile Ammunition;

    protected override void OnFire(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {
        Projectile newAmmo = Instantiate(
            original: Ammunition,
            position: shooter.GetProjectilesSpawnPoint(),
            rotation: Quaternion.identity
      );

        newAmmo.Direction = direction;

        WeaponFireHurtbox hurtbox = newAmmo.GetComponent<WeaponFireHurtbox>();
        hurtbox.Shooter = shooter;
        hurtbox.Weapon = instance;
    }
}
