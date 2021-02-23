using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Projectile Weapon")]
public class ProjectileWeapon : Weapon
{
    public Projectile Ammunition;

    protected override void OnFire(WeaponShooter shooter, Vector2 direction)
    {
        Projectile newAmmo = Instantiate(
            original: Ammunition,
            position: shooter.GetProjectilesSpawnPoint(),
            rotation: Quaternion.identity
      );

        newAmmo.Direction = direction;
    }
}
