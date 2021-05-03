using System.Linq;
using RoomShuffle.Defaults;
using TMPro;

using UnityEngine;

/// <summary>
/// Lets an enemy be hurt by hurtboxes
/// </summary>
public class EnemyHitbox : Hitbox
{
    private HealthController _health;

    private MultiSoundPlayer _multiSoundPlayer;

    private SpotPlayer _spotPlayer;

    /* *** */
    public PopNumber DamageTextPrefab;

    [Tooltip("If true, the hitbox will change the music when hurt")]
    public bool TriggersAdrenalineMusic = true;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override HurtBoxTypes SusceptibleTo { get; } = HurtBoxTypes.HurtfulToEnemies;

    protected override void Awake()
    {
        base.Awake();
        _health = GetComponentInParent<HealthController>();

        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();

        _spotPlayer = transform.parent.transform.Cast<Transform>().FirstOrDefault(x => x.name.Equals("Body"))
            ?.gameObject
            .GetComponent<SpotPlayer>();
    }

    /// <summary>
    /// Creates a red floating label over the enemy's head to signify the damage it took
    /// </summary>
    /// <param name="damage"></param>
    public void CreateDamagePopNumber(int damage)
    {
        const float POP_NUMBER_RANDOM_X_OFFSET = 1f;

        if (damage <= 0)
            return;

        float verticalOffset = 1f;
        float horizontalOffset = RandomValueBetween.Symetrical(POP_NUMBER_RANDOM_X_OFFSET).Pick();

        TextMeshPro instance = Commons.InstantiateInCurrentLevel(
            original: DamageTextPrefab,
            position: transform.position + Vector3.up * verticalOffset + Vector3.right * horizontalOffset
        ).GetComponent<TextMeshPro>();

        string damageText = damage.ToString();

        if (damage >= 10000)
            damageText = "\x221E";

        instance.text = $"-{damageText}";
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="hurtbox"></param>
    protected override void OnReceiveDamage(Hurtbox hurtbox)
    {
        //Play damage sound
        if (_multiSoundPlayer && !(hurtbox is PlayerInvincibilityHurtbox && !Commons.PowerUpManager.HasPowerUp(PowerUp.Invincibility)))
        {
            _multiSoundPlayer.PlaySound(-1, 1, 0.5f);
        }

        if (!(hurtbox is PlayerInvincibilityHurtbox))
            MakeEnemyBlindChase();

        if (TriggersAdrenalineMusic && !(hurtbox is GlobalHurtbox) && hurtbox.GetDamage(this) != 0)
            Commons.SoundtrackPlayer.AddTrigger(this, 5f);

        //Dead men don't scream
        if (_health.IsDead)
            return;

        //Give I-frames
        GrantInvincibilityFrames();

        //Deal damage
        int damage = hurtbox.GetDamage(this);
        _health.DealDamage(damage);

        //Create damage pop-up
        CreateDamagePopNumber(damage);
        
        if (_health.IsDead)
        {
            //Explode
            foreach (var i in GetComponentsInChildren<ParticleExplosion>())
                i.ExplodeBig();

            //Drop items
            foreach (var i in GetComponentsInChildren<DropLootTableOnDeath>())
                i.DropItem();

            //Destroy the object
            Destroy(transform.parent.gameObject);
        }
        else
        {
            if (hurtbox.IgnoresInvincibilityFrames)
                return;

            //Drop some scraps
            foreach (var i in GetComponentsInChildren<ParticleExplosion>())
                i.ExplodeSmall();
        }
            
    }

    private void MakeEnemyBlindChase()
    {
        if (!_spotPlayer)
            return;

        _spotPlayer.UpdateBlindChaseDirection();
        _spotPlayer.BlindChaseTimeLeft = 1.5f;
    }
}
