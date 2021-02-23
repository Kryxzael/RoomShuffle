using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public Vector2 Direction { get; set; }
    public float Speed;
}
