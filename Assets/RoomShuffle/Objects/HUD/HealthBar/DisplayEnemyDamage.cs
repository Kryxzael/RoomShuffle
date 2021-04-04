using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;

/// <summary>
/// Displays a hurtbox's damage
/// </summary>
[RequireComponent(typeof(TextMeshPro))]
public class DisplayEnemyDamage : MonoBehaviour
{
    private TextMeshPro _text;
    public HurtBox Hurtbox;

    private int _cachedDamage;

    private void Start()
    {
        _text = GetComponent<TextMeshPro>();
        RecacheDamage();
    }

    private void FixedUpdate()
    {
        _text.text = _cachedDamage.ToString();

        if (Commons.PlayerHealth.Health <= _cachedDamage)
            _text.color = Color.red;
    }

    public void RecacheDamage()
    {
        _cachedDamage = Hurtbox.GetDamage(FindObjectOfType<PlayerHitbox>());
    }
}
