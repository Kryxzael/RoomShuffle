using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

using UnityEngine;

/// <summary>
/// A static class with references to various game management resources
/// </summary>
public static class Commons
{

    private static RenewableLazy<RoomGenerator> _roomGenerator { get; } = new RenewableLazy<RoomGenerator>(
        () => Object.FindObjectOfType<RoomGenerator>()
    );

    private static RenewableLazy<RoomEffectController> _roomEffectController { get; } = new RenewableLazy<RoomEffectController>(
        () => Object.FindObjectOfType<RoomEffectController>()
    );

    /* *** */

    private static RenewableLazy<GameObject> _playerManager { get; } = new RenewableLazy<GameObject>(
        () => GameObject.Find("PlayerManager")
    );

    private static RenewableLazy<PlayerProgression> _playerProgression { get; } = new RenewableLazy<PlayerProgression>(
        () => PlayerManager.GetComponent<PlayerProgression>()
    );

    private static RenewableLazy<HealthController> _playerHealth { get; } = new RenewableLazy<HealthController>(
        () => PlayerManager.GetComponent<HealthController>()
    );

    private static RenewableLazy<Inventory> _inventory { get; } = new RenewableLazy<Inventory>(
        () => PlayerManager.GetComponent<Inventory>()
    );

    private static RenewableLazy<PowerUpManager> _powerupManager { get; } = new RenewableLazy<PowerUpManager>(
        () => Object.FindObjectOfType<PowerUpManager>()
    );

    /* *** */

    private static RenewableLazy<GameObject> _enemyMananger { get; } = new RenewableLazy<GameObject>(
        () => GameObject.Find("EnemyManager")
    );

    private static RenewableLazy<EnemyProgression> _enemyProgression { get; } = new RenewableLazy<EnemyProgression>(
        () => EnemyManager.GetComponent<EnemyProgression>()
    );
    
    /* *** */
    
    private static RenewableLazy<GameObject> _countdownTimerUI { get; } = new RenewableLazy<GameObject>(
        () => GameObject.FindWithTag("CountdownTimer")
    );
    
    private static RenewableLazy<GameObject> _redCoinsCountdownTimerUI { get; } = new RenewableLazy<GameObject>(
        () => GameObject.FindWithTag("RedCoinsCountdownTimer")
    );
    
    private static RenewableLazy<GameObject> _currencyUI { get; } = new RenewableLazy<GameObject>(
        () => GameObject.FindWithTag("CurrencyText")
    );

    /*
     * Room Generation
     */

    /// <summary>
    /// Gets the current room effects if a room effect controller is present
    /// </summary>
    public static RoomEffects CurrentRoomEffects
    {
        get
        {
            if (RoomGenerator != null)
                return RoomGenerator.CurrentRoomConfig?.Effect ?? RoomEffects.None;

            return RoomEffects.None;
        }
    }

    /// <summary>
    /// Gets the room generator of the scene
    /// </summary>
    public static RoomGenerator RoomGenerator => _roomGenerator.Value;

    /// <summary>
    /// Gets the room effect controller of the scene
    /// </summary>
    public static RoomEffectController RoomEffectController => _roomEffectController.Value;

    /*
     * Player
     */

    /// <summary>
    /// Gets the player manager object of the scene
    /// </summary>
    public static GameObject PlayerManager => _playerManager.Value;

    /// <summary>
    /// Gets the player progression controller of the scene
    /// </summary>
    public static PlayerProgression PlayerProgression => _playerProgression.Value;

    /// <summary>
    /// Gets the player's health controller
    /// </summary>
    public static HealthController PlayerHealth => _playerHealth.Value;

    /// <summary>
    /// Gets the player's inventory
    /// </summary>
    public static Inventory Inventory => _inventory.Value;

    public static PowerUpManager PowerUpManager => _powerupManager.Value;

    /*
     * HUD
     */
    
    /// <summary>
    /// Gets the CountdownTimer in UI
    /// </summary>
    public static GameObject CountdownTimer => _countdownTimerUI.Value;
    
    /// <summary>
    /// Gets the CountdownTimer in UI
    /// </summary>
    public static GameObject RedCoinsCountdownTimer => _redCoinsCountdownTimerUI.Value;
    
    /// <summary>
    /// Gets the currency UI element
    /// </summary>
    public static GameObject CurrencyUI => _currencyUI.Value;
    

    /*
     * Enemies
     */

    /// <summary>
    /// Gets the enemy manager object of the scene
    /// </summary>
    public static GameObject EnemyManager => _enemyMananger.Value;

    /// <summary>
    /// Gets the enemy progression controller of the scene
    /// </summary>
    public static EnemyProgression EnemyProgression => _enemyProgression.Value;

    /// <summary>
    /// Spawns the provided object and parents it to the currently loaded level
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="original"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public static T InstantiateInCurrentLevel<T>(T original, Vector3 position, Quaternion? rotation = null) where T : Object
    {
        GeneratedRoom parent = Object.FindObjectOfType<GeneratedRoom>();
        Transform transform = null;

        if (parent != null)
            transform = parent.transform;

        return Object.Instantiate(original, position, rotation ?? Quaternion.identity, transform);
    }

    /// <summary>
    /// Respawns the player at the start of the room
    /// </summary>
    public static void RespawnPlayer()
    {
        var player = CommonExtensions.GetPlayer();
        var spawnPoint = Vector2.zero;

        if (Object.FindObjectOfType<Entrance>() is Entrance entr)
            spawnPoint = entr.transform.position;

        player.transform.position = spawnPoint;
        player.GetComponent<Rigidbody2D>().velocity = default;
    }

    /// <summary>
    /// Scales a float variable to its required value using the provided mode if applicable
    /// </summary>
    /// <returns></returns>
    public static float GetEffectValue(float originalValue, EffectValueType inputType)
    {
        RoomEffects fxs = CurrentRoomEffects;
        RoomEffectController controller = RoomEffectController;

        if (controller == null)
            return originalValue;

        switch (inputType)
        {
            case EffectValueType.EnemySpeed:
                if (PowerUpManager.HasPowerUp(PowerUp.SlowDown))
                    return originalValue * PowerUpManager.SlowDownTimeScale;

                if (fxs.HasFlag(RoomEffects.FastFoe))
                    return originalValue * controller.FastFoeSpeedMultiplier;
                break;
            case EffectValueType.EnemyWaitTime:
                if (PowerUpManager.HasPowerUp(PowerUp.SlowDown))
                    return originalValue / PowerUpManager.SlowDownTimeScale;

                if (fxs.HasFlag(RoomEffects.FastFoe))
                    return originalValue / controller.FastFoeSpeedMultiplier;

                break;
            case EffectValueType.ProjectileSize:
                if (fxs.HasFlag(RoomEffects.LargeProjectiles))
                    return originalValue * controller.LargeProjectilesGrowMultiplier;
                break;

            case EffectValueType.PlayerAcceleration:
                if (fxs.HasFlag(RoomEffects.Icy))
                    return originalValue * controller.IcyGroundAccelerationMultiplier;
                break;

            case EffectValueType.PlayerDeceleration:
                if (fxs.HasFlag(RoomEffects.Icy))
                    return originalValue * controller.IcyGroundDecelerationMultiplier;
                break;


            case EffectValueType.PlayerMaxSpeed:
                if (fxs.HasFlag(RoomEffects.Icy))
                    return originalValue * controller.IcyGroundMaxSpeedMultiplier;
                break;
        }

        return originalValue;
    }

    /// <summary>
    /// Holds various collider masks
    /// </summary>
    public static class Masks
    {
        public static readonly LayerMask GroundOnly = LayerMask.GetMask("Ground", "Crate");
        public static readonly LayerMask HitboxesHurtboxes = LayerMask.GetMask("Hitbox Hurtbox");
    }
}

/// <summary>
/// The type of scaling that will be applied to a value (if the associated effect is enabled)
/// </summary>
public enum EffectValueType
{
    /// <summary>
    /// The value describes the speed of an enemy.
    /// * The value is upscaled when fast-foe is enabled
    /// </summary>
    EnemySpeed,

    /// <summary>
    /// The value describes how long an enemy waits for something
    /// * The value is downscaled when fast-foe is enabled
    /// </summary>
    EnemyWaitTime,

    /// <summary>
    /// The value describes a component of a projectile's size
    /// * The value will be upscaled when larger projectiles is enabled
    /// </summary>
    ProjectileSize,

    /// <summary>
    /// The value describes the player's acceleration speed
    /// * The value will be reduced when the icy effect is enabled
    /// </summary>
    PlayerAcceleration,

    /// <summary>
    /// The value describes the player's deceleration speed
    /// * The value will be reduced when the icy effect is enabled
    /// </summary>
    PlayerDeceleration,

    /// <summary>
    /// The value describes the player's maximum speed when grounded
    /// * The value will be increased when the icy effect is enabled
    /// </summary>
    PlayerMaxSpeed
}
