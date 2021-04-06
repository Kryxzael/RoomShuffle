using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Weapons/Automatic Wall Shot")]
public class AutomaticWallShot : WeaponTemplate
{
    [Tooltip("The ammunition to use when firing")]
    public List<Projectile> AmmunitionList;

    [Tooltip("The height of the wall")]
    public float Height;

    [Tooltip("How many bullets will be fired in one blast")]
    public int BulletsPerShot;
    
    [Tooltip("How many shots will be fired")]
    public int NumberOfShots;
    
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
        shooter.StartCoroutine(CoShootBullets(instance, shooter, direction));
    }
    
    /// <summary>
    /// Shoots bullets with identical time between each bullet
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="shooter"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator CoShootBullets(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {
        for (int a = 0; a < NumberOfShots; a++)
        {
            
            for (int i = 0; i < BulletsPerShot; i++)
            {
                foreach (Projectile projectile in AmmunitionList)
                {

                    Vector2 position = ForceLinear ? fireingPoint : shooter.GetProjectilesSpawnPoint();

                    Vector3 newAmmoPosition;
                    if (direction == Vector2.right || direction == Vector2.left)
                    {
                        newAmmoPosition = new Vector3
                        (position.x,
                            position.y + Height / BulletsPerShot * i);
                    }
                    else
                    {
                        newAmmoPosition = new Vector3
                        (position.x + (Height / BulletsPerShot * i - Height / 2),
                            position.y);
                    }

                    Projectile newAmmo = Instantiate(
                        original: projectile,
                        position: newAmmoPosition,
                        rotation: Quaternion.identity
                    );

                    newAmmo.Direction = direction;

                    WeaponFireHurtbox hurtbox = newAmmo.GetComponentInChildren<WeaponFireHurtbox>();
                    hurtbox.Shooter = shooter;
                    hurtbox.Weapon = instance;

                    newAmmo.Direction = ForceLinear ? fireingDirectionOrigin : shooter.GetCurrentAimingDirection();

                }
            }

            //Waiting for next bullet
            yield return new WaitForSeconds(WaitTime);
        }
    }
}