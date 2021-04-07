using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTimer : MonoBehaviour
{
    [Tooltip("For how long the gameobject will be active before selfdestructing")]
    public float LivingTime;

    private float _time;

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time > LivingTime)
        {
            Destroy(gameObject);
        }
    }
}
