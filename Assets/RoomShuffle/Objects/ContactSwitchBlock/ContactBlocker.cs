using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBlocker : MonoBehaviour
{
    private ContactSwitch _contactSwitch;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        _contactSwitch = GetComponentInParent<ContactSwitch>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _contactSwitch.TriggerSwitch(other);
    }
}
