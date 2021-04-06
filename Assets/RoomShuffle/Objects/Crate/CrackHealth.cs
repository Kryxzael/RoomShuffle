using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Displays a cracking texture based on an object's health
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class CrackHealth : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private HealthController _health;

    /* *** */
    public List<Sprite> CrackingSprites = new List<Sprite>();

    private void Awake()
    {
        _health = GetComponentInParent<HealthController>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float healthPercent = (float)_health.Health / _health.MaximumHealth;
        int highestIndex = CrackingSprites.Count - 1;


        //If a DIV/0 occurs (NaN for 0/0, Infinity for n/0 where n != 0)
        if (float.IsNaN(healthPercent) || float.IsInfinity(healthPercent))
            return;

        _renderer.sprite = CrackingSprites[highestIndex - (int)(healthPercent * highestIndex)];
    }
}
