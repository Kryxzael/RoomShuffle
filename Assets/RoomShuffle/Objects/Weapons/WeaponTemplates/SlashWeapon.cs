using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Assertions.Must;

/// <summary>
/// A weapon that shoots multiple bullets from 90 degrees to 0 degrees
/// </summary>
[CreateAssetMenu(menuName = "Weapons/Slash Weapon")]
public class SlashWeapon : WeaponTemplate
{
    [Tooltip("The ammunition to use when firing")]
    public Projectile Ammunition;

    [Tooltip("How many bullets will be fired in one blast")]
    public int ClusterCount;
    
    [Tooltip("Tiem between each shot")]
    public float WaitTime = 0.03f;

    private Direction1D horizontalDirection;

    protected override void OnFire(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {
        horizontalDirection = shooter.gameObject.GetComponent<Flippable>().Direction;
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

            Vector2 Aim = shooter.GetCurrentAimingDirection();
            
            
            // Decides how much each bullet should ne rotated
            if (Aim == Vector2.left)
            {
                newAmmo.transform.Rotate(((90f / ((float)ClusterCount -1)) * i));   
            } 
            else if (Aim == Vector2.right)
            {
                newAmmo.transform.Rotate(((90f / ((float)ClusterCount -1)) * i) * -1);   
            } 
            else if (Aim == Vector2.up)
            {
                if (horizontalDirection == Direction1D.Left)
                {
                    newAmmo.transform.Rotate(((180f / ((float)ClusterCount-1)) * i) * -1 + 90); 
                }
                else
                {
                    newAmmo.transform.Rotate(((-180f / ((float)ClusterCount-1)) * i) * -1 - 90); 
                }
            }
            else if (Aim == Vector2.down)
            {
                if (horizontalDirection == Direction1D.Left)
                {
                    newAmmo.transform.Rotate(((-180f / ((float)ClusterCount-1)) * i) * -1 + 90); 
                }
                else
                {
                    newAmmo.transform.Rotate(((180f / ((float)ClusterCount-1)) * i) * -1 - 90); 
                }
            }

            WeaponFireHurtbox hurtbox = newAmmo.GetComponent<WeaponFireHurtbox>();
            hurtbox.Shooter = shooter;
            hurtbox.Weapon = instance;
            
            //Waiting for next bullet
            yield return new WaitForSeconds(WaitTime);
        }
    }


}