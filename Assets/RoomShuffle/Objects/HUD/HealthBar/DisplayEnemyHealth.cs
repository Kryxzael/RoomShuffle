using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayEnemyHealth : EnemyScript
{
    private int _lastHealth;
    private TextMeshPro _healthBarText;
    private Transform _healthBar;
    private float _healthBarLoaclScaleX;
    private TextSmack _textSmack;
    

    void Start()
    {
        _healthBar = transform.Find("HealthSlider");
        _healthBarText = transform.Find("HealthText").GetComponent<TextMeshPro>();
        _textSmack = _healthBarText.GetComponent<TextSmack>();
        
        if (!_healthBar)
            throw new Exception("Healthbar not found");

        _healthBarLoaclScaleX = _healthBar.localScale.x;

        _lastHealth = Enemy.HealthController.Health;
        SetDisplayHealth(_lastHealth);
    }

    void Update()
    {
        int currentHealth = Enemy.HealthController.Health;

        if (currentHealth != _lastHealth)
        {
            SetDisplayHealth(currentHealth);
            _lastHealth = currentHealth;
        }
    }

    /// <summary>
    /// Sets the health display to an integer
    /// </summary>
    /// <param name="health"></param>
    public void SetDisplayHealth(int health)
    {
        SetHealthText(health);
        SetFillAmount(health);
    }

    /// <summary>
    /// Sets the health text
    /// </summary>
    /// <param name="health"></param>
    private void SetHealthText(int health)
    {
        _healthBarText.text = health.ToString();
        _textSmack.Smack();
    }

    /// <summary>
    /// sets the fill amount of the health bar. from 0 to 1
    /// </summary>
    /// <param name="percentage"></param>
    private void SetFillAmount(int health)
    {

        _healthBar.localScale = new Vector3(
            health * _healthBarLoaclScaleX / Mathf.Max(1f, Enemy.HealthController.MaximumHealth), //Max-ing to prevent DIV/0
            _healthBar.localScale.y, 
            _healthBar.localScale.z
        );
        
    }
}






