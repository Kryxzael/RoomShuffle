using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class SpawnItemDebugPage : DebugPage
{
    public override string Header { get; } = "Spawn Item";

    protected override void RunItems(DebugMenu caller)
    {
        foreach (GameObject i in caller.SpawnableItems.OrderBy(i => i.name))
        {
            if (Button(i.name))
                Commons.InstantiateInCurrentLevel(i, CommonExtensions.GetPlayer().transform.position + Vector3.right * 2f);
        }
    }
}
