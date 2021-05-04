using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Assertions.Must;

/// <summary>
/// Allows a player to spawn an enemy
/// </summary>
public class SpawnEnemyDebugPage : DebugPage
{
    public override string Header { get; } = "Spawn Enemy";

    protected override void RunItems(DebugMenu caller)
    {
        ReadOnly("- Grounded -");
        //Cycle ground enemies
        foreach (var i in Commons.RoomGenerator.RoomParameterBuilder.GroundEnemies.OrderBy(i => i.Enemy.name))
        {
            //Spawn enemy if button is pressed
            if (Button(i.Enemy.name))
                Commons.InstantiateInCurrentLevel(i.Enemy, CommonExtensions.GetPlayer().transform.position + Vector3.right * 2f);
        }

        Separator();

        ReadOnly("- Flying -");
        //Cycle ground enemies
        foreach (var i in Commons.RoomGenerator.RoomParameterBuilder.AirEnemies.OrderBy(i => i.Enemy.name))
        {
            //Spawn enemy if button is pressed
            if (Button(i.Enemy.name))
                Commons.InstantiateInCurrentLevel(i.Enemy, CommonExtensions.GetPlayer().transform.position + Vector3.right * 2f);
        }
    }
}
