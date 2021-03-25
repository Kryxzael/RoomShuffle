using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// An object that pushes objects on it in a direction
/// </summary>
[RequireComponent(typeof(Flippable))]
public class Conveyor : DetectObjectsOn
{
    private Flippable _flippable;

    [Tooltip("The speed the conveyor will move at")]
    public float Speed;

    private void Awake()
    {
        _flippable = GetComponent<Flippable>();
    }

    private void LateUpdate()
    {
        foreach (Transform i in ObjectsOn)
        {
            i.Translate(_flippable.DirectionVector * Commons.GetEffectValue(Speed, EffectValueType.EnemySpeed));
        }
    }
}
