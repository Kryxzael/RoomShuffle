using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the flood level of a room with a flood-level controller
/// </summary>
public class FloodLevelChanger : MonoBehaviour
{
    [Tooltip("If enabled, the level index property will be added to the current flood level")]
    public bool RelativeChange;

    [Tooltip("The flood level index to change to or by")]
    public int LevelIndex;
}
