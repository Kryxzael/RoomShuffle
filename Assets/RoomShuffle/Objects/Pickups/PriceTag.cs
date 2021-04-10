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
    private void Start()
    {
        var pickup = GetComponentInParent<PickupBase>();
        var textMesh = GetComponent<TextMeshPro>();

        //The price tag should not be visible if the item is free
        if (pickup.Price <= 0)
            gameObject.SetActive(false);

        //The index on the sprite of the sprite to display
        const int SPRITE_SHEET_INDEX = 0;

        //Sets the price text
        textMesh.text = $"{pickup.Price} <sprite={SPRITE_SHEET_INDEX}> ";
    }
}
