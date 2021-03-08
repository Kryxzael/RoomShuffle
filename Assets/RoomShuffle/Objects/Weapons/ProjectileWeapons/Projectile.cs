using Assets.RoomShuffle.Objects.Health.HurtBoxes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

[RequireComponent(typeof(WeaponFireHurtbox))]
public abstract class Projectile : MonoBehaviour
{
    private WeaponFireHurtbox _hurtbox;

    public Vector2 Direction { get; set; }
    public float Speed;

    protected virtual void Awake()
    {
        _hurtbox = GetComponent<WeaponFireHurtbox>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
