using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactSwitch : MonoBehaviour
{

    public bool On = false;
    public Sprite OnSprite;
    public Sprite OffSprite;
    public Sprite NeutralSprite;
    public bool TriggerOnTop = true;
    public bool TriggerOnSides = true;
    public bool TriggerOnBottom = true;

    private List<GameObject> _triggerBoxes = new List<GameObject>();

    private SpriteRenderer _sprite;
    private bool _locked;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.sprite = On ? OnSprite : OffSprite;

        //Combinations of the booleans
        if (TriggerOnTop && !TriggerOnSides && !TriggerOnBottom)
        {
            TopOnly();
        }

        else if (!TriggerOnTop && TriggerOnSides && !TriggerOnBottom)
        {
            SidesOnly();
        }

        else if (TriggerOnTop && TriggerOnSides && !TriggerOnBottom)
        {
            TopAndSides();
        }

        else if (!TriggerOnTop && TriggerOnSides && TriggerOnBottom)
        {
            BottomAndSides();
        }
        
        else if (!TriggerOnTop && !TriggerOnSides && TriggerOnBottom)
        {
            Bottom();
        }
        
        else if (TriggerOnTop && !TriggerOnSides && TriggerOnBottom)
        {
            TopAndBottom();
        }
        else
        {
           All(); 
        }

        
    }

    public void TriggerSwitch(Collider2D collider)
    {
        if (_locked)
            return;

        if (collider.gameObject.IsPlayer())
        {
            On = !On;

            _sprite.sprite = On ? OnSprite : OffSprite;
        }
    }

    public void Lock()
    {
        _sprite.sprite = NeutralSprite;
        _locked = true;
    }

    private void TopOnly()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Top"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void SidesOnly()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Sides"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void TopAndSides()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("TopAndSides"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void BottomAndSides()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("BottomAndSides"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void All()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("All"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }
    
    private void Bottom()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Bottom"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }
    
    private void TopAndBottom()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("TopAndBottom"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }
}
