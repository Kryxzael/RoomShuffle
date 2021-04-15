using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactSwitch : MonoBehaviour
{

    public bool On = false;
    public Sprite OnSprite;
    public Sprite OffSprite;

    private SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.sprite = On ? OnSprite : OffSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.IsPlayer())
        {
            On = !On;

            _sprite.sprite = On ? OnSprite : OffSprite;
        }
    }
}
