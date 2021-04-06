using UnityEngine;


[CreateAssetMenu(menuName = "Weapons/Wall Shot")]
public class WallShot : WeaponTemplate
{
    [Tooltip("The ammunition to use when firing")]
    public Projectile Ammunition;

    [Tooltip("The height of the wall")]
    public float Height;

    [Tooltip("How many bullets will be fired in one blast")]
    public int BlastCount;

    protected override void OnFire(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {
        for (int i = 0; i < BlastCount; i++)
        {
            Vector3 newAmmoPosition;
            if (direction == Vector2.right || direction == Vector2.left)
            {
                newAmmoPosition = new Vector3
                (shooter.GetProjectilesSpawnPoint().x,
                    shooter.GetProjectilesSpawnPoint().y + Height / BlastCount * i);
            }
            else
            {
                newAmmoPosition = new Vector3
                (shooter.GetProjectilesSpawnPoint().x + (Height / BlastCount * i - Height/2),
                    shooter.GetProjectilesSpawnPoint().y);
            }

            Projectile newAmmo = Instantiate(
                original: Ammunition,
                position: newAmmoPosition,
                rotation: Quaternion.identity
            );

            newAmmo.Direction = direction;

            WeaponFireHurtbox hurtbox = newAmmo.GetComponentInChildren<WeaponFireHurtbox>();
            hurtbox.Shooter = shooter;
            hurtbox.Weapon = instance;
        }
    }
}