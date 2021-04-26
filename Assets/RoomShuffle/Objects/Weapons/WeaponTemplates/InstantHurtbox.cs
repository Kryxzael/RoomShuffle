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

    [Tooltip("The distance away from the player the hurtbox will spawn")]
    public float Distance = 2f;

    protected override void OnFire(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {
        float rotation = Ammunition.name.Equals("InstantHurtbox") ? UnityEngine.Random.Range(0f, 360f) : 0f;

        Projectile newAmmo = Instantiate(
            original: Ammunition,
            position: shooter.GetProjectilesSpawnPoint() + (direction * Distance),
            rotation: Quaternion.Euler(0f, 0f, rotation)
        );

        if (rotation == 0)
        {
            newAmmo.Direction = direction;   
        }

        WeaponFireHurtbox hurtbox = newAmmo.GetComponentInChildren<WeaponFireHurtbox>();
        hurtbox.Shooter = shooter;
        hurtbox.Weapon = instance;

        newAmmo.transform.eulerAngles = newAmmo.transform.eulerAngles.SetY(0);
    }
}