﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using SysRandom = System.Random;

/// <summary>
/// Provides parameter generation services for a room generator
/// </summary>
public abstract class ParameterBuilder : ScriptableObject
{
    /// <summary>
    /// The generator's available rooms
    /// </summary>
    public RoomLayoutCollection Rooms;

    /// <summary>
    /// Gets the next room parameters to use when generating a room
    /// </summary>
    /// <param name="history"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public abstract RoomParameters GetNextParameters(RoomHistory history, SysRandom random);
}
