using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class with references to various game management resources
/// </summary>
public static class Commons
{

    private static RenewableLazy<RoomGenerator> _roomGenerator { get; } = new RenewableLazy<RoomGenerator>(
        () => Object.FindObjectOfType<RoomGenerator>()
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

    /* *** */

    private static RenewableLazy<GameObject> _enemyMananger { get; } = new RenewableLazy<GameObject>(
        () => GameObject.Find("EnemyManager")
    );

    private static RenewableLazy<EnemyProgression> _enemyProgression { get; } = new RenewableLazy<EnemyProgression>(
        () => EnemyManager.GetComponent<EnemyProgression>()
    );

    /*
     * Room Generation
     */

    /// <summary>
    /// Gets the room generator of the scene
    /// </summary>
    public static RoomGenerator RoomGenerator => _roomGenerator.Value;

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
}
