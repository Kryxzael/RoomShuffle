using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Renders the price of a pickup
/// </summary>
[RequireComponent(typeof(TextMeshPro))]
public class PriceTag : MonoBehaviour
{
    private PickupBase _pickup;
    private TextMeshPro _text;

    private void Start()
    {
        _pickup = GetComponentInParent<PickupBase>();
        _text = GetComponent<TextMeshPro>();

        //The price tag should not be visible if the item is free
        if (_pickup.Price <= 0)
            gameObject.SetActive(false);

        //The index on the sprite of the sprite to display
        const int SPRITE_SHEET_INDEX = 0;

        //Sets the price text
        _text.text = $"{_pickup.Price} <sprite={SPRITE_SHEET_INDEX}> ";
    }

    private void Update()
    {
        //Makes the color of the label red if the player can't afford the item
        if (_pickup.Price > Commons.Inventory.Currency)
            _text.color = Color.red;

        else
            _text.color = Color.white;
    }
}
