using UnityEngine;

/// <summary>
/// A weapon that shoots multiple bullets forwards in a cone
/// </summary>
[CreateAssetMenu(menuName = "Weapons/Spraying Weapon")]
public class SprayingWeapon : WeaponTemplate
{
    [Tooltip("The ammunition to use when firing")]
    public Projectile Ammunition;

    [Tooltip("The largest angle offset the bullet can have from the direction the shooter is actually facing")]
    [Range(0, 360)] public int MaxAngle;

    [Tooltip("How many bullets will be fired in one blast")]
    [Range(1, 10)] public int BlastCount = 2;
    

    protected override void OnFire(WeaponInstance instance, WeaponShooterBase shooter, Vector2 direction)
    {

        for (int i = 0; i < BlastCount; i++)
        {

            Projectile newAmmo = Instantiate(
                original: Ammunition,
                position: shooter.GetProjectilesSpawnPoint(),
                rotation: Quaternion.identity
            );

            //Rotates the bullet random amount
            RandomValueBetween randomAngle = new RandomValueBetween(-MaxAngle/2, MaxAngle/2);
            int angle = randomAngle.PickInt();
            newAmmo.Direction = direction;
            newAmmo.transform.Rotate(angle);

            WeaponFireHurtbox hurtbox = newAmmo.GetComponent<WeaponFireHurtbox>();
            hurtbox.Shooter = shooter;
            hurtbox.Weapon = instance;
        }
    }
}