using System.Linq;

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

    [Tooltip("The number that will appear when the enemy receives damage")]
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
        _spotPlayer = GetComponentInChildren<SpotPlayer>();
    }

    /// <summary>
    /// Creates a red floating label over the enemy's head to signify the damage it took
    /// </summary>
    /// <param name="damage"></param>
    public void CreateDamagePopNumber(int damage)
    {
        //By how much the number can be randomly offset horizontally
        const float POP_NUMBER_RANDOM_X_OFFSET = 1f;
        const string INFINITY_SYMBOL = "\x221E";

        //Don't show pop-up if no damage was taken
        if (damage <= 0)
            return;

        //Calculate offsets
        float verticalOffset = 1f;
        float horizontalOffset = RandomValueBetween.Symetrical(POP_NUMBER_RANDOM_X_OFFSET).Pick();

        //Spawn object
        TextMeshPro instance = Commons.InstantiateInCurrentLevel(
            original: DamageTextPrefab,
            position: transform.position + Vector3.up * verticalOffset + Vector3.right * horizontalOffset
        ).GetComponent<TextMeshPro>();

        //Set damage text
        string damageText = damage.ToString();

        if (damage >= 10000)
            damageText = INFINITY_SYMBOL;

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
            Commons.SoundtrackPlayer.AddAdrenalineTrigger(this, 5f);

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

    /// <summary>
    /// Makes an enemy with a SpotPlayer script start blind-chasing when fired at
    /// </summary>
    private void MakeEnemyBlindChase()
    {
        if (!_spotPlayer)
            return;

        _spotPlayer.UpdateBlindChaseDirection();
        _spotPlayer.BlindChaseTimeLeft = 1.5f;
    }
}
