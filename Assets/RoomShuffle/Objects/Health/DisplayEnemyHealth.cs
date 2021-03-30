using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayEnemyHealth : MonoBehaviour
{
    private HealthController _healthController;
    private int _lastHealth;
    private TextMeshPro TMP;
    private Transform _healthBar;
    private float _healthBarLoaclScaleX;
    private TextSmack _textSmack;
    

    void Start()
    {
        _healthController = transform.parent.GetComponentInChildren<HealthController>();
        _healthBar = transform.Find("HealthSlider");
        TMP = transform.Find("HealthText").GetComponent<TextMeshPro>();
        _textSmack = TMP.GetComponent<TextSmack>();
        
        if (!_healthController || !_healthBar)
            throw new Exception("Healthbar or HealthController not found");

        _healthBarLoaclScaleX = _healthBar.localScale.x;
        

    }

    void Update()
    {
        int health = _healthController.Health;
        
        if (_lastHealth != health)
        {
            _lastHealth = health;
            
            SetDisplayHealth(health);
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
        TMP.text = health.ToString();
        _textSmack.Smack();
    }

    /// <summary>
    /// sets the fill amount of the health bar. from 0 to 1
    /// </summary>
    /// <param name="percentage"></param>
    private void SetFillAmount(int health)
    {
        _healthBar.localScale = new Vector3(
            _healthController.Health * _healthBarLoaclScaleX / _healthController.MaximumHealth,
            _healthBar.localScale.y, 
            _healthBar.localScale.z
            );
        
    }
}






