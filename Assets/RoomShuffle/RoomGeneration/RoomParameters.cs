using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class RoomParameters
{
    public RoomClass Class;
    public RoomLayout Layout;
    public RoomTheme Theme;
    public RoomEffects Effect;
    public EnemySet EnemySet;
    public bool FlipHorizontal;
    public IEnumerator<WeaponTemplate> WeaponEnumerator;
}
