using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// An object that pushes objects on it in a direction
/// </summary>
public class Conveyor : DetectObjectsOn
{
    [Tooltip("The direction the conveyor will move its objects")]
    public Direction1D Directionality;

    [Tooltip("The speed the conveyor will move at")]
    public float Speed;

    private void LateUpdate()
    {
        foreach (Transform i in ObjectsOn)
        {
            i.Translate(new Vector3((int)Directionality * Commons.GetEffectValue(Speed, EffectValueType.EnemySpeed), 0f));
        }
    }
}
