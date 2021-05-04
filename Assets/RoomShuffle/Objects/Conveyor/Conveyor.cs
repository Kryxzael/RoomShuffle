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
    //The default speed of the conveyor that the animation is linked to
    private const float BASE_BELT_SPEED = 5f;

    /* *** */

    private Flippable _flippable;

    /* *** */

    [Tooltip("The speed the conveyor will move at")]
    public float Speed = BASE_BELT_SPEED;

    private void Awake()
    {
        _flippable = GetComponent<Flippable>();
    }

    private void Start()
    {
        //Configure animation speed
        var animator = GetComponent<SpriteAnimator>();
        animator.PlaybackSpeed = Speed / BASE_BELT_SPEED;
    }

    private void LateUpdate()
    {
        //Move all objects on the the conveyor
        foreach (Transform i in ObjectsOn)
        {
            i.Translate(_flippable.DirectionVector * Commons.GetEffectValue(Speed, EffectValueType.EnemySpeed) * Time.deltaTime);
        }
    }
}
