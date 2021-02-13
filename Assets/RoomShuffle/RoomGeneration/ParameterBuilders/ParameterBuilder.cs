using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using SysRandom = System.Random;

public abstract class ParameterBuilder : ScriptableObject
{
    public RoomLayoutCollection Rooms;

    public abstract RoomParameters GetNextParameters(RoomHistory history, SysRandom random);
}
