using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Allows the player to spawn an object
/// </summary>
public class SpawnItemDebugPage : DebugPage
{
    public override string Header { get; } = "Spawn Item";

    protected override void RunItems(DebugMenu caller)
    {
        //Cycle spawnable objects
        foreach (GameObject i in caller.SpawnableItems.OrderBy(i => i.name))
        {
            //Create a button for the object and spawn the object is clicked
            if (Button(i.name))
                Commons.InstantiateInCurrentLevel(i, CommonExtensions.GetPlayer().transform.position + Vector3.right * 2f);
        }
    }
}
