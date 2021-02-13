using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HideSpriteOnPlay : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = null;
    }
}
