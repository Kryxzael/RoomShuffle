using UnityEngine;

/// <summary>
/// A weapon that shoots multiple bullets forwards in a cone
/// </summary>
[CreateAssetMenu(menuName = "Weapons/Cone Shot")]
public class ConeShotWeapon : WeaponTemplate
{
    [Tooltip("The ammunition to use when firing")]
    public Projectile Ammunition;

    [Tooltip("The largest angle offset the bullet can have from the direction the shooter is actually facing")]
    public float MaxAngle;

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
            newAmmo.transform.Rotate((i - BlastCount / 2) * MaxAngle);

            WeaponFireHurtbox hurtbox = newAmmo.GetComponentInChildren<WeaponFireHurtbox>();
            hurtbox.Shooter = shooter;
            hurtbox.Weapon = instance;
        }
    }
}