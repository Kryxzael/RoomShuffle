using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class RoomParameters
{
    public RoomClass Class;
    public RoomLayout Layout;
    public RoomTheme Theme;
    public RoomEffects Effect;
    public bool FlipHorizontal;
    public IEnumerator<WeaponTemplate> WeaponEnumerator;
    public List<EnemyWithSpawnRate> GroundEnemies;
    public List<EnemyWithSpawnRate> AirEnemies;
    public EntranceExitSides Entrance;
    public EntranceExitSides Exit;

    [Serializable]
    public class EnemyWithSpawnRate
    {
        public EnemyBase Enemy;

        [Range(0f, 1f)]
        public float SpawnRate = 1f;
    }
}
