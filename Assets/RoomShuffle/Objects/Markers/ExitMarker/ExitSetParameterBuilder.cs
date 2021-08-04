using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// An exit that sets the active room generator when used by the player
/// </summary>
public class ExitSetParameterBuilder : Exit
{
    [Tooltip("The builder to set")]
    public ParameterBuilder Builder;

    public bool ReloadLevel;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
        {
            Commons.RoomGenerator.History.Clear();
            Commons.RoomGenerator.RoomParameterBuilder = Builder;
            base.OnTriggerEnter2D(collision);
        }
    }
}
