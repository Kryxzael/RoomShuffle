using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A weapon that shoots mulitple bullets in burst linearly with two kinds of bullets
/// </summary>
[CreateAssetMenu(menuName = "Weapons/Double Automatic Shot")]
public class DoubleBulletAutomatic : WeaponTemplate
{
    [Tooltip("The ammunition to use when firing (primary)")]
    public List<Projectile> AmmunitionList;

    [Tooltip("How many bullets will be fired in one blast")]
    public int ClusterCount;
    
    [Tooltip("Time between each shot")]
    public float WaitTime = 0.05f;
    
    [Tooltip("Forces the bullets to appear after each other")]
    public bool ForceLinear = false;

    private Vector2 fireingPoint = Vector2.zero;
    private Vector2 fireingDirectionOrigin = Vector2.zero;

    protected override void OnFire(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {
        fireingPoint = shooter.gameObject.transform.position;
        fireingDirectionOrigin = shooter.GetCurrentAimingDirection();
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
            foreach (Projectile projectile in AmmunitionList)
            {

                //Decide the origin of each bullet
                Vector2 position = ForceLinear ? fireingPoint : shooter.GetProjectilesSpawnPoint();

                Projectile newAmmo = Instantiate(
                    original: projectile,
                    position: position,
                    rotation: Quaternion.identity
                );

                newAmmo.Direction = ForceLinear ? fireingDirectionOrigin : shooter.GetCurrentAimingDirection();

                WeaponFireHurtbox hurtbox = newAmmo.GetComponentInChildren<WeaponFireHurtbox>();
                hurtbox.Shooter = shooter;
                hurtbox.Weapon = instance;
            }

            //Waiting for next bullet
            yield return new WaitForSeconds(WaitTime);
        }
    }
}