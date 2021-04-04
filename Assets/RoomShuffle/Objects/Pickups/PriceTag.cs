using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceTag : MonoBehaviour
{
    private TextMeshPro TMP;

    private void Awake()
    {
        TMP = GetComponent<TextMeshPro>();
    }

    public void SetPrice(int price)
    {
        TMP.text = price + "<sprite=4>";
    }
}
