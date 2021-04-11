using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys the object a certain amount of time after it was created
/// </summary>
public class DespawnAfterTime : MonoBehaviour
{
    [Tooltip("The time it will take for the object to despawn")]
    public int DespawnTime;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(DespawnTime);
        Destroy(gameObject);
    }
}
