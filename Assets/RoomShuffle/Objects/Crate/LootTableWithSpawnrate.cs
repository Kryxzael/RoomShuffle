using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[Serializable]
public class LootTableWithSpawnrate
{
    [Tooltip("The likelihood of this table being selected")]
    [Range(0f, 1f)]
    public float Spawnrate = 1f;

    [Tooltip("The loot table")]
    public LootTable Table;
}
