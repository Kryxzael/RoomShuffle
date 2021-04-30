using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Assertions.Must;

public class SpawnEnemyDebugPage : DebugPage
{
    public override string Header { get; } = "Spawn Enemy";

    protected override void RunItems(DebugMenu caller)
    {
        ReadOnly("- Grounded -");
        foreach (var i in Commons.RoomGenerator.RoomParameterBuilder.GroundEnemies.OrderBy(i => i.Enemy.name))
        {
            if (Button(i.Enemy.name))
                Commons.InstantiateInCurrentLevel(i.Enemy, CommonExtensions.GetPlayer().transform.position + Vector3.right * 2f);
        }

        Separator();
        ReadOnly("- Flying -");
        foreach (var i in Commons.RoomGenerator.RoomParameterBuilder.AirEnemies.OrderBy(i => i.Enemy.name))
        {
            if (Button(i.Enemy.name))
                Commons.InstantiateInCurrentLevel(i.Enemy, CommonExtensions.GetPlayer().transform.position + Vector3.right * 2f);
        }
    }
}
