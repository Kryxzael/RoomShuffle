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

    //The damage the enemy will deal
    private int _cachedDamage;

    private void Start()
    {
        _text = GetComponent<TextMeshPro>();
        RecacheDamage();
    }

    private void FixedUpdate()
    {
        _text.text = _cachedDamage.ToString();

        //sets the text to red if the enemies damage will kill you
        if (Commons.PlayerHealth.Health <= _cachedDamage)
            _text.color = Color.red;
    }

    /// <summary>
    /// Finds how much damage the enemy will deal
    /// </summary>
    public void RecacheDamage()
    {
        _cachedDamage = Hurtbox.GetDamage(FindObjectOfType<PlayerHitbox>());
    }
}
